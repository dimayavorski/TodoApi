using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;
using TodoApi.Application.Common.Options.Aws;

namespace TodoApi.Infrastructure.Services.Aws
{
    public class AwsMessageConsumerService : IMessageConsumerService
    {
        private IAmazonSQS _amazonSqs;
        private readonly ILogger<AwsMessageConsumerService> _logger;
        private readonly IMediator _mediator;
        private readonly SqsOptions _sqsOptions;
        private string? QueueUrl;
        public AwsMessageConsumerService(ILogger<AwsMessageConsumerService> logger, IAmazonSQS amazonSQS, IMediator mediator, IOptions<SqsOptions> options)
        {
            _sqsOptions = options.Value ?? throw new ArgumentNullException(nameof(SqsOptions));
            _logger = logger;
            _mediator = mediator;
            _amazonSqs = amazonSQS;
        }

        public async Task Connect()
        {

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

    }
}

