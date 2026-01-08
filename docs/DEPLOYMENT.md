# Deployment Guide for Diagnostic08FinalCommit

This guide provides comprehensive instructions for deploying the Diagnostic08FinalCommit .NET 8.0 ASP.NET Core application to AWS ECS Fargate using containerization.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Overview](#project-overview)
3. [Local Development Setup](#local-development-setup)
4. [Docker Deployment](#docker-deployment)
5. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
6. [ECS Fargate Setup](#ecs-fargate-setup)
7. [ECS Task Definition Explained](#ecs-task-definition-explained)
8. [ECS Service Configuration](#ecs-service-configuration)
9. [ECS Fargate Deployment Walkthrough](#ecs-fargate-deployment-walkthrough)
10. [Configuration Management](#configuration-management)
11. [Security Considerations](#security-considerations)
12. [ECS-Specific Troubleshooting](#ecs-specific-troubleshooting)
13. [ECS Fargate Scaling and Management](#ecs-fargate-scaling-and-management)
14. [Technology-Specific Notes](#technology-specific-notes)

---

## Prerequisites

### Required Software

- **.NET 8.0 SDK** or later
- **Docker Desktop** (20.10 or later)
- **AWS CLI** (v2.x or later)
- **Git** for version control
- **Visual Studio 2022** or **VS Code** (optional, for development)

### Required AWS Resources

- AWS Account with appropriate permissions
- IAM user with programmatic access
- AWS CLI configured with credentials

### System Requirements

- **Operating System**: Windows 10/11, macOS, or Linux
- **Memory**: Minimum 8GB RAM (16GB recommended)
- **Disk Space**: At least 10GB free space

---

## Project Overview

**Diagnostic08FinalCommit** is a .NET 8.0 ASP.NET Core Web API application designed for containerized deployment on AWS ECS Fargate.

### Technology Stack

- **Framework**: .NET 8.0 ASP.NET Core
- **Application Type**: Web API
- **Build Tool**: dotnet CLI
- **Package Type**: Framework-dependent deployment
- **Application Port**: 8080
- **Health Endpoint**: /health

### Key Features

- RESTful API endpoints
- Health check middleware for monitoring
- Structured logging and diagnostics
- Production-ready containerization
- AWS ECS Fargate optimized deployment

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Diagnostic08FinalCommit
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Application

```bash
dotnet build -c Release
```

### 4. Run Locally

```bash
dotnet run
```

The application will start on `http://localhost:8080`.

### 5. Test Health Endpoint

```bash
curl http://localhost:8080/health
```

Expected response: `200 OK` with health status.

---

## Docker Deployment

### Build Docker Image Locally

```bash
docker build -t diagnostic08finalcommit:latest .
```

### Run Container Locally

```bash
docker run -d -p 8080:8080 --name diagnostic08finalcommit diagnostic08finalcommit:latest
```

### Test Containerized Application

```bash
curl http://localhost:8080/health
```

### Using Docker Compose

```bash
docker-compose up -d
```

This starts the application with all configured environment variables and volumes.

### Stop and Remove Containers

```bash
docker-compose down
```

---

## AWS ECS Fargate Prerequisites

### 1. Install and Configure AWS CLI

#### Install AWS CLI

**Windows**:
```powershell
msiexec.exe /i https://awscli.amazonaws.com/AWSCLIV2.msi
```

**macOS**:
```bash
brew install awscli
```

**Linux**:
```bash
curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
unzip awscliv2.zip
sudo ./aws/install
```

#### Configure AWS CLI

```bash
aws configure
```

Provide:
- AWS Access Key ID
- AWS Secret Access Key
- Default region (e.g., us-east-1)
- Default output format (json)

### 2. Create VPC and Networking Resources

#### Create VPC

```bash
aws ec2 create-vpc --cidr-block 10.0.0.0/16 --region us-east-1
```

Note the VPC ID from the output.

#### Create Subnets

Create at least 2 subnets in different availability zones:

```bash
aws ec2 create-subnet --vpc-id <VPC_ID> --cidr-block 10.0.1.0/24 --availability-zone us-east-1a
aws ec2 create-subnet --vpc-id <VPC_ID> --cidr-block 10.0.2.0/24 --availability-zone us-east-1b
```

#### Create Internet Gateway

```bash
aws ec2 create-internet-gateway
aws ec2 attach-internet-gateway --vpc-id <VPC_ID> --internet-gateway-id <IGW_ID>
```

#### Create Route Table

```bash
aws ec2 create-route-table --vpc-id <VPC_ID>
aws ec2 create-route --route-table-id <ROUTE_TABLE_ID> --destination-cidr-block 0.0.0.0/0 --gateway-id <IGW_ID>
aws ec2 associate-route-table --route-table-id <ROUTE_TABLE_ID> --subnet-id <SUBNET_ID>
```

### 3. Create Security Group

```bash
aws ec2 create-security-group --group-name ecs-sg --description "ECS Security Group" --vpc-id <VPC_ID>
```

#### Add Inbound Rules

```bash
# Allow HTTP traffic on port 8080
aws ec2 authorize-security-group-ingress --group-id <SG_ID> --protocol tcp --port 8080 --cidr 0.0.0.0/0

# Allow HTTP traffic on port 80 (for ALB)
aws ec2 authorize-security-group-ingress --group-id <SG_ID> --protocol tcp --port 80 --cidr 0.0.0.0/0
```

### 4. Create IAM Roles

#### ECS Task Execution Role

Create a trust policy file `ecs-trust-policy.json`:

```json
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
```

Create the role:

```bash
aws iam create-role --role-name ecsTaskExecutionRole --assume-role-policy-document file://ecs-trust-policy.json
```

Attach the managed policy:

```bash
aws iam attach-role-policy --role-name ecsTaskExecutionRole --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (Optional)

For applications that need to access AWS services:

```bash
aws iam create-role --role-name ecsTaskRole --assume-role-policy-document file://ecs-trust-policy.json
```

Attach necessary policies based on your application requirements.

---

## ECS Fargate Setup

### 1. Create ECR Repository

```bash
aws ecr create-repository --repository-name diagnostic08finalcommit --region us-east-1
```

### 2. Create CloudWatch Log Group

```bash
aws logs create-log-group --log-group-name /ecs/diagnostic08finalcommit --region us-east-1
```

### 3. Create ECS Cluster

```bash
aws ecs create-cluster --cluster-name my-ecs-cluster --region us-east-1
```

---

## ECS Task Definition Explained

The task definition is a blueprint for your application. Key components:

### Fargate Requirements

- **requiresCompatibilities**: ["FARGATE"] - Specifies Fargate launch type
- **networkMode**: "awsvpc" - Required for Fargate, provides each task with its own ENI

### CPU and Memory

**Valid Fargate CPU/Memory Combinations**:

| CPU (vCPU) | Memory (MB) |
|------------|-------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024, 2048, 3072, 4096 |
| 1024 (1)   | 2048-8192 (increments of 1024) |
| 2048 (2)   | 4096-16384 (increments of 1024) |
| 4096 (4)   | 8192-30720 (increments of 1024) |

**Default Configuration**: CPU: "512", Memory: "1024"

### IAM Roles

- **executionRoleArn**: Allows ECS to pull images from ECR and write logs to CloudWatch
- **taskRoleArn**: Grants permissions to the application (e.g., access to S3, DynamoDB)

### Container Definitions

- **name**: Container name used in service definition
- **image**: Docker image URI from ECR
- **essential**: If true, task stops if this container stops
- **portMappings**: Only containerPort is specified for Fargate (no hostPort)

### Logging Configuration

```json
"logConfiguration": {
  "logDriver": "awslogs",
  "options": {
    "awslogs-group": "/ecs/diagnostic08finalcommit",
    "awslogs-region": "us-east-1",
    "awslogs-stream-prefix": "ecs"
  }
}
```

---

## ECS Service Configuration

The service definition manages running tasks and integrates with load balancers.

### Launch Type

- **launchType**: "FARGATE" - Serverless compute engine

### Network Configuration

```json
"networkConfiguration": {
  "awsvpcConfiguration": {
    "subnets": ["subnet-xxx", "subnet-yyy"],
    "securityGroups": ["sg-xxx"],
    "assignPublicIp": "ENABLED"
  }
}
```

- **subnets**: Must be in different availability zones for high availability
- **securityGroups**: Control inbound/outbound traffic
- **assignPublicIp**: ENABLED if tasks need internet access

### Deployment Configuration

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

- **maximumPercent**: Maximum tasks during deployment (200 = 2x desired count)
- **minimumHealthyPercent**: Minimum healthy tasks during deployment (50 = half)
- **deploymentCircuitBreaker**: Automatically rolls back failed deployments

### Load Balancer Configuration

```json
"loadBalancers": [
  {
    "targetGroupArn": "arn:aws:elasticloadbalancing:...",
    "containerName": "diagnostic08finalcommit",
    "containerPort": 8080
  }
],
"healthCheckGracePeriodSeconds": 300
```

- **targetGroupArn**: ALB target group (must use target-type: ip)
- **healthCheckGracePeriodSeconds**: Time before health checks start (allows app startup)

### Tags Configuration

**CRITICAL**: Use "tags" parameter, NOT "serviceTags" (invalid and causes deployment failure)

```json
"tags": [
  {"key": "Environment", "value": "production"},
  {"key": "Application", "value": "diagnostic08finalcommit"}
]
```

---

## ECS Fargate Deployment Walkthrough

### Step 1: Build and Push Docker Image

#### Linux/macOS

```bash
cd /path/to/Diagnostic08FinalCommit
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

#### Windows

```cmd
cd C:\path\to\Diagnostic08FinalCommit
scripts\build-push.bat
```

**Script Actions**:
1. Prompts for registry type (AWS ECR or Docker Hub)
2. Sanitizes image name and tag
3. Authenticates with selected registry
4. Creates ECR repository if it doesn't exist
5. Builds Docker image
6. Pushes image to registry

### Step 2: Deploy to ECS Fargate

#### Linux/macOS

```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

#### Windows

```cmd
scripts\deploy-image.bat
```

**Script Actions**:
1. Prompts for AWS region, cluster name, VPC, subnets, security group
2. Prompts for Docker image URI
3. Checks/creates ECS cluster
4. Prompts for load balancer (y/n)
5. If yes: Creates ALB, target group (target-type: ip), and listener
6. Creates CloudWatch log group
7. Registers task definition with provided image URI
8. Creates or updates ECS service
9. Waits for service to stabilize
10. Displays deployment details and application URL

### Step 3: Verify Deployment

#### Check Service Status

```bash
aws ecs describe-services --cluster my-ecs-cluster --services diagnostic08finalcommit-service --region us-east-1
```

#### View Running Tasks

```bash
aws ecs list-tasks --cluster my-ecs-cluster --service-name diagnostic08finalcommit-service --region us-east-1
```

#### Access Application

If load balancer was created, access via ALB DNS name:

```bash
curl http://<ALB_DNS>/health
```

#### View Logs

```bash
aws logs tail /ecs/diagnostic08finalcommit --follow --region us-east-1
```

---

## Configuration Management

### Environment Variables

Environment variables are defined in the task definition:

```json
"environment": [
  {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"},
  {"name": "ASPNETCORE_URLS", "value": "http://+:8080"}
]
```

### Secrets Management

For sensitive data, use AWS Secrets Manager:

```json
"secrets": [
  {
    "name": "DB_PASSWORD",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:db-password"
  }
]
```

### Configuration Files

Mount configuration files using EFS (optional):

1. Create EFS file system
2. Add volume definition in task definition
3. Mount to container path

---

## Security Considerations

### 1. Non-Root User

The Dockerfile creates a non-root user for enhanced security:

```dockerfile
RUN groupadd -r appuser && useradd -r -g appuser appuser
USER appuser
```

### 2. IAM Roles

- Use least privilege principle
- Separate execution role from task role
- Regularly audit permissions

### 3. Security Groups

- Restrict inbound traffic to necessary ports
- Use security group rules instead of CIDR blocks where possible
- Regularly review and update rules

### 4. Network Isolation

- Use private subnets for tasks when possible
- Use NAT Gateway for outbound internet access
- Implement VPC endpoints for AWS services

### 5. Secrets Management

- Never hardcode secrets in Dockerfile or environment variables
- Use AWS Secrets Manager or Parameter Store
- Rotate credentials regularly

### 6. Image Scanning

Enable ECR image scanning:

```bash
aws ecr put-image-scanning-configuration --repository-name diagnostic08finalcommit --image-scanning-configuration scanOnPush=true
```

---

## ECS-Specific Troubleshooting

### Task Failures

#### Issue: Task stops immediately after starting

**Diagnosis**:
```bash
aws ecs describe-tasks --cluster my-ecs-cluster --tasks <TASK_ARN> --region us-east-1
```

**Common Causes**:
- Application crash (check logs)
- Health check failure
- Invalid task definition
- Insufficient permissions

**Solution**:
- Check CloudWatch logs for application errors
- Verify health endpoint responds correctly
- Validate task definition syntax
- Ensure execution role has necessary permissions

### Network Issues

#### Issue: Cannot reach application via ALB

**Diagnosis**:
```bash
aws elbv2 describe-target-health --target-group-arn <TG_ARN> --region us-east-1
```

**Common Causes**:
- Security group not allowing traffic
- Health check failing
- Subnets not configured correctly

**Solution**:
- Verify security group allows inbound on port 8080
- Check target health status
- Ensure subnets have route to internet gateway

### CPU/Memory Errors

#### Issue: Task fails with CPU/memory errors

**Error**: "Invalid CPU or memory value specified"

**Solution**:
- Verify CPU/memory combination is valid for Fargate
- Use valid combinations (see ECS Task Definition Explained section)
- Default safe values: cpu: "512", memory: "1024"

### Service Deployment Stuck

#### Issue: Service deployment doesn't complete

**Diagnosis**:
```bash
aws ecs describe-services --cluster my-ecs-cluster --services diagnostic08finalcommit-service --region us-east-1
```

**Common Causes**:
- New tasks failing health checks
- Insufficient capacity in cluster
- Deployment circuit breaker triggered

**Solution**:
- Check task stopped reason
- Review health check configuration
- Increase health check grace period
- Check deployment events in service description

### CloudWatch Logs Not Appearing

#### Issue: No logs in CloudWatch

**Common Causes**:
- Log group doesn't exist
- Execution role missing CloudWatch permissions
- Log configuration incorrect

**Solution**:
```bash
# Create log group
aws logs create-log-group --log-group-name /ecs/diagnostic08finalcommit --region us-east-1

# Verify execution role has logs:CreateLogStream and logs:PutLogEvents permissions
```

---

## ECS Fargate Scaling and Management

### Service Auto Scaling

#### Enable Auto Scaling

```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
    --service-namespace ecs \
    --resource-id service/my-ecs-cluster/diagnostic08finalcommit-service \
    --scalable-dimension ecs:service:DesiredCount \
    --min-capacity 2 \
    --max-capacity 10 \
    --region us-east-1

# Create scaling policy
aws application-autoscaling put-scaling-policy \
    --service-namespace ecs \
    --resource-id service/my-ecs-cluster/diagnostic08finalcommit-service \
    --scalable-dimension ecs:service:DesiredCount \
    --policy-name cpu-scaling-policy \
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
  "ScaleOutCooldown": 60,
  "ScaleInCooldown": 60
}
```

### Blue/Green Deployments

For zero-downtime deployments:

1. Use AWS CodeDeploy with ECS
2. Configure deployment group
3. Specify test listener for validation
4. Automatic traffic shifting

### Monitoring

#### CloudWatch Metrics

- CPUUtilization
- MemoryUtilization
- TargetResponseTime
- HealthyHostCount

#### Set Up Alarms

```bash
aws cloudwatch put-metric-alarm \
    --alarm-name high-cpu-diagnostic08finalcommit \
    --alarm-description "Alert when CPU exceeds 80%" \
    --metric-name CPUUtilization \
    --namespace AWS/ECS \
    --statistic Average \
    --period 300 \
    --threshold 80 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 2 \
    --region us-east-1
```

---

## Technology-Specific Notes

### .NET 8.0 ASP.NET Core Considerations

#### Kestrel Configuration

Kestrel is configured to listen on port 8080:

```csharp
ENV ASPNETCORE_URLS=http://+:8080
```

#### Health Checks

Implement ASP.NET Core health checks:

```csharp
app.MapHealthChecks("/health");
```

#### Logging

Use structured logging with Serilog or Microsoft.Extensions.Logging:

```csharp
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});
```

#### Application Insights (Optional)

For advanced monitoring:

```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

#### Graceful Shutdown

ASP.NET Core handles SIGTERM signals automatically. Configure timeout:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
});
```

#### Culture and Globalization

```dockerfile
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
```

This ensures proper culture and timezone handling in containers.

---

## Conclusion

This deployment guide provides comprehensive instructions for containerizing and deploying the Diagnostic08FinalCommit .NET 8.0 application to AWS ECS Fargate. Follow the steps carefully, and refer to the troubleshooting section for common issues.

For additional support:
- AWS ECS Documentation: https://docs.aws.amazon.com/ecs/
- .NET Docker Documentation: https://docs.microsoft.com/en-us/dotnet/core/docker/
- ASP.NET Core Documentation: https://docs.microsoft.com/en-us/aspnet/core/

**Happy Deploying!**