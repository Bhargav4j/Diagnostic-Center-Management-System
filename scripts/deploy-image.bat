@echo off
setlocal enabledelayedexpansion

echo =====================================
echo AWS ECS Fargate Deployment Script
echo =====================================
echo.

REM Project configuration
set PROJECT_NAME=diagnostic08finalcommit
set TASK_FAMILY=!PROJECT_NAME!-task
set SERVICE_NAME=!PROJECT_NAME!-service

REM Prompt for AWS configuration
echo === AWS Configuration ===
set /p AWS_REGION="Enter AWS Region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS Cluster Name (e.g., my-ecs-cluster): "

echo.
echo === Network Configuration ===
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNET_INPUT="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "

REM Parse subnet IDs
for /f "tokens=1,2 delims=," %%a in ("!SUBNET_INPUT!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
set /p IMAGE_URI="Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): "

echo.
echo Getting AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo Account ID: !ACCOUNT_ID!

echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating ECS cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
)

echo.
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

if /i "!NEED_LB!"=="y" (
    echo.
    echo === Creating Application Load Balancer ===
    
    echo Creating Application Load Balancer...
    for /f "delims=" %%i in ('aws elbv2 create-load-balancer --name !PROJECT_NAME!-alb --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --region !AWS_REGION! --query LoadBalancers[0].LoadBalancerArn --output text') do set ALB_ARN=%%i
    
    echo Load Balancer ARN: !ALB_ARN!
    
    echo Waiting for load balancer to be active...
    aws elbv2 wait load-balancer-available --load-balancer-arns !ALB_ARN! --region !AWS_REGION!
    
    echo Creating Target Group...
    for /f "delims=" %%i in ('aws elbv2 create-target-group --name !PROJECT_NAME!-tg --protocol HTTP --port 8080 --vpc-id !VPC_ID! --target-type ip --health-check-enabled --health-check-path /health --health-check-interval-seconds 30 --health-check-timeout-seconds 5 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region !AWS_REGION! --query TargetGroups[0].TargetGroupArn --output text') do set TARGET_GROUP_ARN=%%i
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    echo Creating Listener...
    aws elbv2 create-listener --load-balancer-arn !ALB_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul
    
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns !ALB_ARN! --region !AWS_REGION! --query LoadBalancers[0].DNSName --output text') do set ALB_DNS=%%i
    
    echo Load Balancer DNS: !ALB_DNS!
    
    set USE_LOAD_BALANCER=true
) else (
    set USE_LOAD_BALANCER=false
    set TARGET_GROUP_ARN=
)

echo.
echo Creating CloudWatch Log Group...
aws logs create-log-group --log-group-name /ecs/!PROJECT_NAME! --region !AWS_REGION! >nul 2>&1

echo.
echo Updating task definition with configuration...

REM Replace placeholders in task definition using PowerShell
powershell -Command "(Get-Content ecs\task-definition.json) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' -replace '{{AWS_REGION}}', '!AWS_REGION!' -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content ecs\task-definition.json"

echo Registering ECS task definition...
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://ecs/task-definition.json --region !AWS_REGION! --query taskDefinition.taskDefinitionArn --output text') do set TASK_DEF_ARN=%%i

echo Task Definition ARN: !TASK_DEF_ARN!

echo.
echo Updating service definition with configuration...

REM Replace placeholders in service definition
powershell -Command "(Get-Content ecs\service-definition.json) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' -replace '{{SUBNET_1}}', '!SUBNET_1!' -replace '{{SUBNET_2}}', '!SUBNET_2!' -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' | Set-Content ecs\service-definition.json"

if "!USE_LOAD_BALANCER!"=="false" (
    powershell -Command "$json = Get-Content ecs\service-definition.json | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content ecs\service-definition.json"
) else (
    powershell -Command "(Get-Content ecs\service-definition.json) -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content ecs\service-definition.json"
)

echo.
echo Checking if service exists...
for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query services[?status==`ACTIVE`].serviceName --output text 2^>nul') do set EXISTING_SERVICE=%%i

if "!EXISTING_SERVICE!"=="" (
    echo Service does not exist. Creating new service...
    aws ecs create-service --cli-input-json file://ecs/service-definition.json --region !AWS_REGION! >nul
    echo Service created successfully
) else (
    echo Service exists. Updating service...
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --force-new-deployment --region !AWS_REGION! >nul
    echo Service updated successfully
)

echo.
echo Waiting for service to stabilize...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo =====================================
echo Deployment Completed Successfully
echo =====================================
echo.
echo Service Details:
aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query services[0].{Name:serviceName,Status:status,DesiredCount:desiredCount,RunningCount:runningCount} --output table

echo.
if "!USE_LOAD_BALANCER!"=="true" (
    echo Application URL: http://!ALB_DNS!
    echo.
)
echo CloudWatch Logs: /ecs/!PROJECT_NAME!
echo.
echo To view logs run:
echo aws logs tail /ecs/!PROJECT_NAME! --follow --region !AWS_REGION!
echo.

endlocal