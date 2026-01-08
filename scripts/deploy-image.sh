#!/bin/bash
set -e
set -o pipefail

echo "====================================="
echo "AWS ECS Fargate Deployment Script"
echo "====================================="
echo ""

# Project configuration
PROJECT_NAME="diagnostic08finalcommit"
TASK_FAMILY="${PROJECT_NAME}-task"
SERVICE_NAME="${PROJECT_NAME}-service"

# Prompt for AWS configuration
echo "=== AWS Configuration ==="
read -p "Enter AWS Region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS Cluster Name (e.g., my-ecs-cluster): " CLUSTER_NAME

echo ""
echo "=== Network Configuration ==="
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNET_INPUT
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Parse subnet IDs
IFS=',' read -ra SUBNET_ARRAY <<< "$SUBNET_INPUT"
SUBNET_1=${SUBNET_ARRAY[0]}
SUBNET_2=${SUBNET_ARRAY[1]:-$SUBNET_1}

echo ""
read -p "Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): " IMAGE_URI

echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating ECS cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
}

echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "=== Creating Application Load Balancer ==="
    
    # Create load balancer
    echo "Creating Application Load Balancer..."
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "${PROJECT_NAME}-alb" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text)
    
    echo "Load Balancer ARN: $ALB_ARN"
    
    # Wait for load balancer to be active
    echo "Waiting for load balancer to be active..."
    aws elbv2 wait load-balancer-available --load-balancer-arns "$ALB_ARN" --region "$AWS_REGION"
    
    # Create target group with ip target type for Fargate
    echo "Creating Target Group..."
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "${PROJECT_NAME}-tg" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text)
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    # Create listener
    echo "Creating Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null
    
    # Get load balancer DNS name
    ALB_DNS=$(aws elbv2 describe-load-balancers \
        --load-balancer-arns "$ALB_ARN" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].DNSName' \
        --output text)
    
    echo "Load Balancer DNS: $ALB_DNS"
    
    # Keep loadBalancers in service definition and add healthCheckGracePeriodSeconds
    USE_LOAD_BALANCER="true"
else
    # Remove loadBalancers section from service definition
    USE_LOAD_BALANCER="false"
    TARGET_GROUP_ARN=""
fi

echo ""
echo "Creating CloudWatch Log Group..."
aws logs create-log-group --log-group-name "/ecs/${PROJECT_NAME}" --region "$AWS_REGION" 2>/dev/null || echo "Log group already exists"

echo ""
echo "Updating task definition with configuration..."

# Replace placeholders in task definition
sed -i "s|{{IMAGE_URI}}|${IMAGE_URI}|g" ecs/task-definition.json
sed -i "s|{{AWS_REGION}}|${AWS_REGION}|g" ecs/task-definition.json
sed -i "s|{{ACCOUNT_ID}}|${ACCOUNT_ID}|g" ecs/task-definition.json

echo "Registering ECS task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://ecs/task-definition.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task Definition ARN: $TASK_DEF_ARN"

echo ""
echo "Updating service definition with configuration..."

# Replace placeholders in service definition
sed -i "s|{{CLUSTER_NAME}}|${CLUSTER_NAME}|g" ecs/service-definition.json
sed -i "s|{{SUBNET_1}}|${SUBNET_1}|g" ecs/service-definition.json
sed -i "s|{{SUBNET_2}}|${SUBNET_2}|g" ecs/service-definition.json
sed -i "s|{{SECURITY_GROUP}}|${SECURITY_GROUP}|g" ecs/service-definition.json

if [ "$USE_LOAD_BALANCER" = "false" ]; then
    # Remove loadBalancers and healthCheckGracePeriodSeconds from service definition
    python3 -c "
import json
import sys
with open('ecs/service-definition.json', 'r') as f:
    data = json.load(f)
data.pop('loadBalancers', None)
data.pop('healthCheckGracePeriodSeconds', None)
with open('ecs/service-definition.json', 'w') as f:
    json.dump(data, f, indent=2)
" 2>/dev/null || {
        # Fallback if Python is not available
        echo "Warning: Could not remove loadBalancers section. Please ensure Python 3 is installed."
    }
else
    sed -i "s|{{TARGET_GROUP_ARN}}|${TARGET_GROUP_ARN}|g" ecs/service-definition.json
fi

echo ""
echo "Checking if service exists..."
EXISTING_SERVICE=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[?status==`ACTIVE`].serviceName' \
    --output text 2>/dev/null || echo "")

if [ -z "$EXISTING_SERVICE" ]; then
    echo "Service does not exist. Creating new service..."
    aws ecs create-service \
        --cli-input-json file://ecs/service-definition.json \
        --region "$AWS_REGION" >/dev/null
    echo "Service created successfully"
else
    echo "Service exists. Updating service..."
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION" >/dev/null
    echo "Service updated successfully"
fi

echo ""
echo "Waiting for service to stabilize..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "====================================="
echo "Deployment Completed Successfully"
echo "====================================="
echo ""
echo "Service Details:"
aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].{Name:serviceName,Status:status,DesiredCount:desiredCount,RunningCount:runningCount}' \
    --output table

echo ""
if [ "$USE_LOAD_BALANCER" = "true" ]; then
    echo "Application URL: http://${ALB_DNS}"
    echo ""
fi
echo "CloudWatch Logs: /ecs/${PROJECT_NAME}"
echo ""
echo "To view logs, run:"
echo "aws logs tail /ecs/${PROJECT_NAME} --follow --region ${AWS_REGION}"
echo ""