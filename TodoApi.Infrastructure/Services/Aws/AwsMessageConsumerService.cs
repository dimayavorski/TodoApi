using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;
using TodoApi.Application.Common.Options.Aws;
using TodoApi.Infrastructure.MessageConsumer;

namespace TodoApi.Infrastructure.Services.Aws
{
    public class AwsMessageConsumerService : IMessageConsumerService
    {
        private IAmazonSQS _amazonSqs;
        private readonly ILogger<MessageConsumerFactory> _logger;
        private readonly AwsCredentialsOptions _credentialsOptions;
        private readonly IMediator _mediator;
        private SqsOptions _sqsOptions;
        private string QueueUrl;
        public AwsMessageConsumerService(ILogger<MessageConsumerFactory> logger, IMediator mediator, IOptions<SqsOptions> options, IOptions<AwsCredentialsOptions> credentialsOptions)
        {
            _sqsOptions = options.Value ?? throw new ArgumentNullException(nameof(SqsOptions));
            _credentialsOptions = credentialsOptions.Value ?? throw new ArgumentNullException(nameof(AwsCredentialsOptions));
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Connect()
        {

            _amazonSqs = GetAmazonSQSClient();

            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = _sqsOptions.QueueName
            };
            //Move Queue Creation
            var queueRespnse = await _amazonSqs.CreateQueueAsync(createQueueRequest);
            QueueUrl = queueRespnse.QueueUrl;

            if (queueRespnse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Failed to create queue");
            }
        }

        public async Task ConsumeMessage(CancellationToken cancellationToken)
        {

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                AttributeNames = new List<string> { "All" },
                MessageAttributeNames = new List<string> { "All" },
                MaxNumberOfMessages = 5
            };
            var response = await _amazonSqs.ReceiveMessageAsync(receiveMessageRequest, cancellationToken);
            foreach (var message in response.Messages)
            {
                try
                {
                    
                    var deserealizedMessageType = JsonSerializer.Deserialize<BaseQueueMessage>(message.Body)?.MessageType;
                    var typeNamespace = $"ProfileApi.Infrastructure.Messages.{deserealizedMessageType}";
                    var type = Type.GetType(typeNamespace);
                    if (type == null)
                    {
                        _logger.LogInformation("Message Handler wasn't found");
                        continue;
                    }

                    var typedMessage = (IQueueMessage)JsonSerializer.Deserialize(message.Body, type)!;

                    await _mediator.Send(typedMessage, cancellationToken);

                    var deleteMessageResponse = await _amazonSqs.DeleteMessageAsync(QueueUrl, message.ReceiptHandle, cancellationToken);

                    if (deleteMessageResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogInformation("Failed deleteting message from queue");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private AmazonSQSClient GetAmazonSQSClient() {
            BasicAWSCredentials basicCredentials = new BasicAWSCredentials(_credentialsOptions.AccessKey, _credentialsOptions.Secret);
            RegionEndpoint region = RegionEndpoint.GetBySystemName(_credentialsOptions.AwsRegion);
            return new AmazonSQSClient(basicCredentials, region);
        }
    }
}

