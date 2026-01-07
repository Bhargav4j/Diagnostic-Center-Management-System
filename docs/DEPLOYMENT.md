# AWS ECS Fargate Deployment Guide

## Comp-transformed-Diagnostic007 - .NET 8.0 Application

This guide provides comprehensive instructions for deploying the .NET 8.0 ASP.NET Core application to AWS ECS Fargate.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Architecture Overview](#architecture-overview)
3. [Local Development Setup](#local-development-setup)
4. [Docker Build and Registry Setup](#docker-build-and-registry-setup)
5. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
6. [ECS Task Definition](#ecs-task-definition)
7. [ECS Service Configuration](#ecs-service-configuration)
8. [Deployment Walkthrough](#deployment-walkthrough)
9. [Post-Deployment Verification](#post-deployment-verification)
10. [Monitoring and Logging](#monitoring-and-logging)
11. [Troubleshooting](#troubleshooting)
12. [Scaling and Management](#scaling-and-management)
13. [Security Best Practices](#security-best-practices)

---

## Prerequisites

### Required Tools

- **Docker Desktop** (version 20.10 or higher)
  - Windows: https://docs.docker.com/desktop/windows/install/
  - macOS: https://docs.docker.com/desktop/mac/install/
  - Linux: https://docs.docker.com/engine/install/

- **AWS CLI** (version 2.x)
  ```bash
  # Install AWS CLI v2
  # Windows: Download from https://awscli.amazonaws.com/AWSCLIV2.msi
  # macOS:
  curl "https://awscli.amazonaws.com/AWSCLIV2.pkg" -o "AWSCLIV2.pkg"
  sudo installer -pkg AWSCLIV2.pkg -target /
  # Linux:
  curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
  unzip awscliv2.zip
  sudo ./aws/install
  ```

- **.NET 8.0 SDK** (for local development)
  ```bash
  # Download from https://dotnet.microsoft.com/download/dotnet/8.0
  dotnet --version  # Verify installation
  ```

- **jq** (JSON processor for Linux/macOS scripts)
  ```bash
  # macOS
  brew install jq
  # Linux
  sudo apt-get install jq  # Ubuntu/Debian
  sudo yum install jq      # CentOS/RHEL
  ```

### AWS Account Requirements

- Active AWS account with appropriate permissions
- IAM user with the following permissions:
  - ECS Full Access
  - ECR Full Access
  - IAM Role creation/attachment
  - VPC and networking permissions
  - CloudWatch Logs access
  - Elastic Load Balancing (if using ALB)

### AWS CLI Configuration

```bash
# Configure AWS CLI with your credentials
aws configure
# Enter:
# - AWS Access Key ID
# - AWS Secret Access Key
# - Default region (e.g., us-east-1)
# - Default output format (json)

# Verify configuration
aws sts get-caller-identity
```

---

## Architecture Overview

### Application Stack

- **Runtime**: .NET 8.0 ASP.NET Core
- **Container Base Image**: mcr.microsoft.com/dotnet/aspnet:8.0
- **Application Port**: 80 (HTTP)
- **Health Check Endpoint**: /health
- **Configuration**: appsettings.json, environment variables

### AWS Infrastructure

- **Compute**: AWS ECS Fargate (serverless containers)
- **Networking**: VPC with public/private subnets
- **Load Balancing**: Application Load Balancer (optional)
- **Container Registry**: AWS ECR or Docker Hub
- **Logging**: CloudWatch Logs
- **IAM Roles**: Task Execution Role, Task Role

### ECS Fargate Architecture

```
┌─────────────────────────────────────────────────┐
│              Application Load Balancer          │
│                 (Optional)                      │
└────────────┬────────────────────────┬───────────┘
             │                        │
             v                        v
    ┌────────────────┐       ┌────────────────┐
    │  ECS Task      │       │  ECS Task      │
    │  (Fargate)     │       │  (Fargate)     │
    │                │       │                │
    │  ┌──────────┐  │       │  ┌──────────┐  │
    │  │Container │  │       │  │Container │  │
    │  │.NET App  │  │       │  │.NET App  │  │
    │  └──────────┘  │       │  └──────────┘  │
    └────────────────┘       └────────────────┘
             │                        │
             └────────┬───────────────┘
                      v
            ┌──────────────────┐
            │  CloudWatch Logs │
            └──────────────────┘
```

---

## Local Development Setup

### 1. Clone Repository

```bash
cd /path/to/project
```

### 2. Build Application Locally

```bash
# Restore dependencies
dotnet restore

# Build application
dotnet build -c Release

# Run application locally
dotnet run --project .

# Application should be available at http://localhost:5000
```

### 3. Test with Docker Compose

```bash
# Build and run with Docker Compose
docker-compose up --build

# Application should be available at http://localhost:8080
# Health check: http://localhost:8080/health

# Stop containers
docker-compose down
```

---

## Docker Build and Registry Setup

### Option 1: AWS ECR (Recommended for AWS deployments)

#### Build and Push Script

**Linux/macOS:**
```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

**Windows:**
```cmd
scripts\build-push.bat
```

#### Manual ECR Setup

```bash
# Set variables
AWS_REGION="us-east-1"
AWS_ACCOUNT_ID="123456789012"
REPO_NAME="comp-diagnostic"
IMAGE_TAG="latest"

# Create ECR repository
aws ecr create-repository \
    --repository-name $REPO_NAME \
    --region $AWS_REGION

# Authenticate Docker to ECR
aws ecr get-login-password --region $AWS_REGION | \
    docker login --username AWS --password-stdin \
    $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com

# Build Docker image
docker build -t $REPO_NAME:$IMAGE_TAG .

# Tag image for ECR
docker tag $REPO_NAME:$IMAGE_TAG \
    $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$REPO_NAME:$IMAGE_TAG

# Push to ECR
docker push $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$REPO_NAME:$IMAGE_TAG
```

### Option 2: Docker Hub

```bash
# Login to Docker Hub
docker login -u YOUR_USERNAME

# Build image
docker build -t YOUR_USERNAME/comp-diagnostic:latest .

# Push to Docker Hub
docker push YOUR_USERNAME/comp-diagnostic:latest
```

---

## AWS ECS Fargate Prerequisites

### 1. VPC and Networking Setup

#### Create VPC (if needed)

```bash
# Create VPC
VPC_ID=$(aws ec2 create-vpc \
    --cidr-block 10.0.0.0/16 \
    --region us-east-1 \
    --query 'Vpc.VpcId' \
    --output text)

# Create Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
    --region us-east-1 \
    --query 'InternetGateway.InternetGatewayId' \
    --output text)

# Attach IGW to VPC
aws ec2 attach-internet-gateway \
    --vpc-id $VPC_ID \
    --internet-gateway-id $IGW_ID \
    --region us-east-1

# Create Public Subnets
SUBNET_1=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.1.0/24 \
    --availability-zone us-east-1a \
    --query 'Subnet.SubnetId' \
    --output text)

SUBNET_2=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.2.0/24 \
    --availability-zone us-east-1b \
    --query 'Subnet.SubnetId' \
    --output text)

# Create Route Table
RTB_ID=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)

# Add route to Internet Gateway
aws ec2 create-route \
    --route-table-id $RTB_ID \
    --destination-cidr-block 0.0.0.0/0 \
    --gateway-id $IGW_ID

# Associate subnets with route table
aws ec2 associate-route-table --subnet-id $SUBNET_1 --route-table-id $RTB_ID
aws ec2 associate-route-table --subnet-id $SUBNET_2 --route-table-id $RTB_ID
```

#### Create Security Group

```bash
# Create security group
SG_ID=$(aws ec2 create-security-group \
    --group-name comp-diagnostic-sg \
    --description "Security group for Comp Diagnostic ECS tasks" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

# Allow HTTP traffic (port 80)
aws ec2 authorize-security-group-ingress \
    --group-id $SG_ID \
    --protocol tcp \
    --port 80 \
    --cidr 0.0.0.0/0

# Allow HTTPS traffic (port 443) - if needed
aws ec2 authorize-security-group-ingress \
    --group-id $SG_ID \
    --protocol tcp \
    --port 443 \
    --cidr 0.0.0.0/0
```

### 2. IAM Roles Setup

#### Task Execution Role

```bash
# Create trust policy
cat > task-execution-trust-policy.json <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "ecs-tasks.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF

# Create role
aws iam create-role \
    --role-name ecsTaskExecutionRole \
    --assume-role-policy-document file://task-execution-trust-policy.json

# Attach AWS managed policy
aws iam attach-role-policy \
    --role-name ecsTaskExecutionRole \
    --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### Task Role (Optional - for application AWS access)

```bash
# Create task role
aws iam create-role \
    --role-name ecsTaskRole \
    --assume-role-policy-document file://task-execution-trust-policy.json

# Attach policies as needed (e.g., S3, DynamoDB access)
# aws iam attach-role-policy --role-name ecsTaskRole --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess
```

### 3. CloudWatch Log Group

```bash
# Create log group
aws logs create-log-group \
    --log-group-name /ecs/comp-diagnostic \
    --region us-east-1

# Set retention period (optional, 7 days)
aws logs put-retention-policy \
    --log-group-name /ecs/comp-diagnostic \
    --retention-in-days 7 \
    --region us-east-1
```

---

## ECS Task Definition

### Task Definition Overview

The task definition (`ecs/task-definition.json`) specifies:

- **Family**: comp-diagnostic-task
- **Launch Type**: FARGATE
- **Network Mode**: awsvpc (required for Fargate)
- **CPU**: 512 (.5 vCPU)
- **Memory**: 1024 MB (1 GB)

### Valid Fargate CPU/Memory Combinations

| CPU (units) | vCPU | Memory Options (MB) |
|-------------|------|--------------------|
| 256 | 0.25 | 512, 1024, 2048 |
| 512 | 0.5 | 1024, 2048, 3072, 4096 |
| 1024 | 1 | 2048-8192 (1 GB increments) |
| 2048 | 2 | 4096-16384 (1 GB increments) |
| 4096 | 4 | 8192-30720 (1 GB increments) |

### Container Definition

```json
{
  "name": "comp-diagnostic",
  "image": "{{IMAGE_URI}}",
  "essential": true,
  "portMappings": [
    {
      "containerPort": 80,
      "protocol": "tcp"
    }
  ],
  "environment": [
    {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"},
    {"name": "ASPNETCORE_URLS", "value": "http://+:80"}
  ],
  "logConfiguration": {
    "logDriver": "awslogs",
    "options": {
      "awslogs-group": "/ecs/comp-diagnostic",
      "awslogs-region": "us-east-1",
      "awslogs-stream-prefix": "ecs"
    }
  },
  "healthCheck": {
    "command": ["CMD-SHELL", "curl -f http://localhost:80/health || exit 1"],
    "interval": 30,
    "timeout": 5,
    "retries": 3,
    "startPeriod": 60
  }
}
```

### Register Task Definition Manually

```bash
# Replace placeholders in task definition
sed -i 's|{{IMAGE_URI}}|123456789.dkr.ecr.us-east-1.amazonaws.com/comp-diagnostic:latest|g' ecs/task-definition.json
sed -i 's|{{AWS_REGION}}|us-east-1|g' ecs/task-definition.json
sed -i 's|{{ACCOUNT_ID}}|123456789012|g' ecs/task-definition.json

# Register task definition
aws ecs register-task-definition \
    --cli-input-json file://ecs/task-definition.json \
    --region us-east-1
```

---

## ECS Service Configuration

### Service Definition Overview

The service definition (`ecs/service-definition.json`) configures:

- **Service Name**: comp-diagnostic-service
- **Desired Count**: 2 (number of tasks)
- **Launch Type**: FARGATE
- **Network Configuration**: awsvpc with subnets and security groups
- **Deployment Configuration**: Rolling updates
- **Load Balancer**: Optional ALB integration

### Key Configuration Elements

#### Network Configuration

```json
"networkConfiguration": {
  "awsvpcConfiguration": {
    "subnets": ["subnet-xxxxx", "subnet-yyyyy"],
    "securityGroups": ["sg-xxxxx"],
    "assignPublicIp": "ENABLED"
  }
}
```

#### Deployment Configuration

```json
"deploymentConfiguration": {
  "maximumPercent": 200,
  "minimumHealthyPercent": 50,
  "deploymentCircuitBreaker": {
    "enable": true,
    "rollback": true
  }
}
```

#### Load Balancer Configuration (Optional)

```json
"loadBalancers": [
  {
    "targetGroupArn": "arn:aws:elasticloadbalancing:...",
    "containerName": "comp-diagnostic",
    "containerPort": 80
  }
],
"healthCheckGracePeriodSeconds": 300
```

---

## Deployment Walkthrough

### Step 1: Build and Push Docker Image

```bash
# Linux/macOS
chmod +x scripts/build-push.sh
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

Follow the interactive prompts:
1. Select registry type (ECR or Docker Hub)
2. Enter registry credentials/details
3. Specify image tag (default: latest)
4. Script will build and push the image

### Step 2: Deploy to ECS Fargate

```bash
# Linux/macOS
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh

# Windows
scripts\deploy-image.bat
```

Provide the following information when prompted:

1. **AWS Region**: e.g., us-east-1
2. **ECS Cluster Name**: e.g., my-ecs-cluster
3. **VPC ID**: e.g., vpc-0abc123def456
4. **Subnet IDs**: Comma-separated, e.g., subnet-0abc123,subnet-0def456
5. **Security Group ID**: e.g., sg-0abc123def
6. **Docker Image URI**: Full image URI from Step 1
7. **Load Balancer**: Choose y/n for ALB setup

### Step 3: Monitor Deployment

```bash
# Watch service status
watch -n 5 'aws ecs describe-services \
    --cluster my-ecs-cluster \
    --services comp-diagnostic-service \
    --query "services[0].{Status:status,Running:runningCount,Desired:desiredCount}"'

# View service events
aws ecs describe-services \
    --cluster my-ecs-cluster \
    --services comp-diagnostic-service \
    --query 'services[0].events[:10]'
```

---

## Post-Deployment Verification

### 1. Check Service Status

```bash
aws ecs describe-services \
    --cluster my-ecs-cluster \
    --services comp-diagnostic-service \
    --region us-east-1
```

### 2. List Running Tasks

```bash
aws ecs list-tasks \
    --cluster my-ecs-cluster \
    --service-name comp-diagnostic-service \
    --region us-east-1
```

### 3. Get Task Details

```bash
# Get task ID from previous command
TASK_ID="arn:aws:ecs:..."

aws ecs describe-tasks \
    --cluster my-ecs-cluster \
    --tasks $TASK_ID \
    --region us-east-1
```

### 4. Test Application Endpoints

**Without Load Balancer:**
```bash
# Get task public IP
TASK_IP=$(aws ecs describe-tasks \
    --cluster my-ecs-cluster \
    --tasks $TASK_ID \
    --query 'tasks[0].attachments[0].details[?name==`networkInterfaceId`].value' \
    --output text | xargs -I {} aws ec2 describe-network-interfaces \
    --network-interface-ids {} \
    --query 'NetworkInterfaces[0].Association.PublicIp' \
    --output text)

# Test endpoints
curl http://$TASK_IP/health
```

**With Load Balancer:**
```bash
# Use ALB DNS name provided by deployment script
curl http://comp-diagnostic-alb-xxxxx.us-east-1.elb.amazonaws.com/health
```

---

## Monitoring and Logging

### CloudWatch Logs

#### View Logs via AWS CLI

```bash
# Tail logs
aws logs tail /ecs/comp-diagnostic --follow --region us-east-1

# Filter logs
aws logs filter-log-events \
    --log-group-name /ecs/comp-diagnostic \
    --filter-pattern "ERROR" \
    --region us-east-1

# Get log streams
aws logs describe-log-streams \
    --log-group-name /ecs/comp-diagnostic \
    --order-by LastEventTime \
    --descending \
    --max-items 10 \
    --region us-east-1
```

#### View Logs in AWS Console

1. Navigate to CloudWatch > Logs > Log groups
2. Select `/ecs/comp-diagnostic`
3. View log streams for each task

### CloudWatch Metrics

```bash
# Get CPU utilization
aws cloudwatch get-metric-statistics \
    --namespace AWS/ECS \
    --metric-name CPUUtilization \
    --dimensions Name=ServiceName,Value=comp-diagnostic-service Name=ClusterName,Value=my-ecs-cluster \
    --start-time $(date -u -d '1 hour ago' +%Y-%m-%dT%H:%M:%S) \
    --end-time $(date -u +%Y-%m-%dT%H:%M:%S) \
    --period 300 \
    --statistics Average \
    --region us-east-1

# Get memory utilization
aws cloudwatch get-metric-statistics \
    --namespace AWS/ECS \
    --metric-name MemoryUtilization \
    --dimensions Name=ServiceName,Value=comp-diagnostic-service Name=ClusterName,Value=my-ecs-cluster \
    --start-time $(date -u -d '1 hour ago' +%Y-%m-%dT%H:%M:%S) \
    --end-time $(date -u +%Y-%m-%dT%H:%M:%S) \
    --period 300 \
    --statistics Average \
    --region us-east-1
```

### Application Insights (Optional)

For advanced monitoring, integrate Application Insights:

```csharp
// Add to appsettings.json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-instrumentation-key"
  }
}

// Add NuGet package
// dotnet add package Microsoft.ApplicationInsights.AspNetCore

// Configure in Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

---

## Troubleshooting

### Common Issues and Solutions

#### 1. Task Fails to Start

**Symptoms**: Tasks start then immediately stop

**Diagnosis**:
```bash
# Check stopped tasks
aws ecs list-tasks \
    --cluster my-ecs-cluster \
    --desired-status STOPPED \
    --region us-east-1

# Get stopped task details
aws ecs describe-tasks \
    --cluster my-ecs-cluster \
    --tasks STOPPED_TASK_ARN \
    --region us-east-1
```

**Common Causes**:
- Invalid CPU/memory combination
- Image pull errors (check ECR permissions)
- Application crashes on startup
- Health check failures

**Solutions**:
- Verify task definition CPU/memory values
- Check task execution role has ECR permissions
- Review CloudWatch logs for application errors
- Increase health check `startPeriod` for slow startups

#### 2. Cannot Pull Image from ECR

**Symptoms**: "CannotPullContainerError"

**Solutions**:
```bash
# Verify ECR repository exists
aws ecr describe-repositories --repository-names comp-diagnostic --region us-east-1

# Check task execution role policy
aws iam get-role-policy \
    --role-name ecsTaskExecutionRole \
    --policy-name AmazonECSTaskExecutionRolePolicy

# Verify image exists
aws ecr describe-images \
    --repository-name comp-diagnostic \
    --region us-east-1
```

#### 3. Network Connectivity Issues

**Symptoms**: Tasks running but cannot access external services

**Solutions**:
```bash
# Verify security group rules
aws ec2 describe-security-groups \
    --group-ids sg-xxxxx \
    --region us-east-1

# Check subnet route tables
aws ec2 describe-route-tables \
    --filters "Name=association.subnet-id,Values=subnet-xxxxx" \
    --region us-east-1

# Ensure NAT Gateway/Internet Gateway is configured
# For public subnets, route 0.0.0.0/0 to Internet Gateway
# For private subnets, route 0.0.0.0/0 to NAT Gateway
```

#### 4. Service Not Reaching Steady State

**Symptoms**: Deployment stuck, tasks keep restarting

**Diagnosis**:
```bash
# Check service events
aws ecs describe-services \
    --cluster my-ecs-cluster \
    --services comp-diagnostic-service \
    --region us-east-1 \
    --query 'services[0].events[:10]'
```

**Solutions**:
- Check health check configuration
- Verify application responds on configured port
- Review application logs for errors
- Check load balancer target group health

#### 5. High Memory or CPU Usage

**Diagnosis**:
```bash
# Monitor resource usage
aws ecs describe-tasks \
    --cluster my-ecs-cluster \
    --tasks TASK_ARN \
    --include CONTAINER_INSTANCE_ARN \
    --region us-east-1
```

**Solutions**:
- Increase task CPU/memory allocation
- Profile .NET application for memory leaks
- Enable .NET garbage collection logging
- Consider using .NET ReadyToRun images

---

## Scaling and Management

### Manual Scaling

```bash
# Update desired count
aws ecs update-service \
    --cluster my-ecs-cluster \
    --service comp-diagnostic-service \
    --desired-count 5 \
    --region us-east-1
```

### Auto Scaling Configuration

#### Target Tracking Scaling (CPU)

```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
    --service-namespace ecs \
    --resource-id service/my-ecs-cluster/comp-diagnostic-service \
    --scalable-dimension ecs:service:DesiredCount \
    --min-capacity 2 \
    --max-capacity 10 \
    --region us-east-1

# Create scaling policy
aws application-autoscaling put-scaling-policy \
    --service-namespace ecs \
    --resource-id service/my-ecs-cluster/comp-diagnostic-service \
    --scalable-dimension ecs:service:DesiredCount \
    --policy-name cpu-target-tracking \
    --policy-type TargetTrackingScaling \
    --target-tracking-scaling-policy-configuration file://scaling-policy.json \
    --region us-east-1
```

**scaling-policy.json**:
```json
{
  "TargetValue": 70.0,
  "PredefinedMetricSpecification": {
    "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
  },
  "ScaleInCooldown": 300,
  "ScaleOutCooldown": 60
}
```

### Blue/Green Deployments

For zero-downtime deployments with CodeDeploy:

```bash
# Create deployment group
aws deploy create-deployment-group \
    --application-name AppECS-my-ecs-cluster-comp-diagnostic-service \
    --deployment-group-name comp-diagnostic-dg \
    --service-role-arn arn:aws:iam::ACCOUNT_ID:role/CodeDeployServiceRole \
    --ecs-services clusterName=my-ecs-cluster,serviceName=comp-diagnostic-service \
    --load-balancer-info targetGroupPairInfoList=[...] \
    --deployment-style deploymentType=BLUE_GREEN,deploymentOption=WITH_TRAFFIC_CONTROL
```

### Rolling Updates

Update task definition and force new deployment:

```bash
# Register new task definition revision
aws ecs register-task-definition \
    --cli-input-json file://ecs/task-definition.json \
    --region us-east-1

# Force new deployment
aws ecs update-service \
    --cluster my-ecs-cluster \
    --service comp-diagnostic-service \
    --force-new-deployment \
    --region us-east-1
```

---

## Security Best Practices

### 1. Use Least Privilege IAM Roles

- Task Execution Role: Only ECR and CloudWatch permissions
- Task Role: Only permissions required by application

### 2. Enable VPC Flow Logs

```bash
aws ec2 create-flow-logs \
    --resource-type VPC \
    --resource-ids $VPC_ID \
    --traffic-type ALL \
    --log-destination-type cloud-watch-logs \
    --log-group-name /aws/vpc/flowlogs \
    --deliver-logs-permission-arn arn:aws:iam::ACCOUNT_ID:role/VPCFlowLogsRole
```

### 3. Use Secrets Manager for Sensitive Data

```bash
# Store secret
aws secretsmanager create-secret \
    --name comp-diagnostic/db-password \
    --secret-string "your-secure-password" \
    --region us-east-1

# Reference in task definition
"secrets": [
  {
    "name": "DB_PASSWORD",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:ACCOUNT_ID:secret:comp-diagnostic/db-password"
  }
]
```

### 4. Enable Container Insights

```bash
# Enable for cluster
aws ecs put-account-setting \
    --name containerInsights \
    --value enabled \
    --region us-east-1

# Enable for specific cluster
aws ecs update-cluster-settings \
    --cluster my-ecs-cluster \
    --settings name=containerInsights,value=enabled \
    --region us-east-1
```

### 5. Regular Security Updates

- Rebuild images regularly with latest base images
- Monitor .NET security advisories
- Update dependencies via `dotnet outdated`
- Scan images with AWS ECR image scanning

```bash
# Enable ECR image scanning
aws ecr put-image-scanning-configuration \
    --repository-name comp-diagnostic \
    --image-scanning-configuration scanOnPush=true \
    --region us-east-1
```

---

## Additional Resources

- [AWS ECS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/AWS_Fargate.html)
- [.NET on AWS](https://aws.amazon.com/developer/language/net/)
- [ASP.NET Core Deployment](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [AWS CLI Reference](https://awscli.amazonaws.com/v2/documentation/api/latest/reference/ecs/index.html)

---

## Support and Feedback

For issues or questions:
- Check CloudWatch logs: `/ecs/comp-diagnostic`
- Review ECS service events
- Consult AWS Support
- Review .NET application logs

**Generated**: 2026-01-07
**Version**: 1.0.0
**Platform**: AWS ECS Fargate
**Runtime**: .NET 8.0 ASP.NET Core