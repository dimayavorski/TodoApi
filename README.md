## TodoApi
TodoApi is a simple API endpoint that responsible for maintaining daily tasks
It can be configured to use 2 different cloud stacks Azure/AWS depending on environment variables

## Architecture diagram

![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/Infrastructure.png?raw=true)

## AWS Configuration

### API Gateway
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsApiGateway.png?raw=true)

### Elastic BeanStalk
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsElasticBeanStalk.png?raw=true)

### SNS
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsSNS.png?raw=true)

### SQS
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsSQS.png?raw=true)

### Lambda
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsLambda.png?raw=true)

### DynamoDb
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsDynamoDb.png?raw=true)

### Secrets
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsSecrets.png?raw=true)

### S3
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AwsS3.png?raw=true)

## Azure Configuration

### Api Management Service
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureApiManagement.png?raw=true)

### App Service
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureAppService.png?raw=true)

### Service Bus Topic
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureServiceBusTopic.png?raw=true)

### Service Bus Topic Subscription
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureServiceBusTopicSubscription.png?raw=true)

### Function (ImageProcessor)
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureFunction.png?raw=true)

### CosmosDb
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureCosmosDb.png?raw=true)

### Key Vaults
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureKeyVault.png?raw=true)

### Blob Storage
![alt text](https://github.com/dimayavorski/TodoApi/blob/main/ReadmeContent/AzureBlobStorage.png?raw=true)

## Comparison
Genearally, both cloud providers have mostly the same services.
As a .NET dev, I find Azure more convinient since it has tight integration with Visual Studion.
In other hand, AWS seems a bit cheeper than Azure.
