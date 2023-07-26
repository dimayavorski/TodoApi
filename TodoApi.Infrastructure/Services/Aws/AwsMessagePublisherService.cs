using System;
using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options.AWS;

namespace TodoApi.Infrastructure.Services.Aws
{
    public class AwsMessagePublisherService : IMessagePublisherService
	{
        private readonly IAmazonSimpleNotificationService _sns;
        private readonly AwsSnsOptions _snsOptios;
        private string? _topicArn;

		public AwsMessagePublisherService(IAmazonSimpleNotificationService sns, IOptions<AwsSnsOptions> snsOptions)
		{
            _sns = sns;
            _snsOptios = snsOptions.Value ?? throw new NullReferenceException(nameof(AwsSnsOptions));
		}

        public async Task PublishMessage<T>(T message) where T : IQueueMessage
        {
            var topicArn =  await GetTopicArnAsync();

            var sendMessageRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
            };

            var response = await _sns.PublishAsync(sendMessageRequest);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new ApplicationException("Failed publishing message");
        }

        private async ValueTask<string> GetTopicArnAsync()
        {
                if (_topicArn != null)
                return _topicArn;

            var queueUrlResponse = await _sns.FindTopicAsync(_snsOptios.TopicName);

            if (queueUrlResponse != null)
            {
                _topicArn = queueUrlResponse.TopicArn;
                return _topicArn;
            }

            var createTopicResponse = await _sns.CreateTopicAsync(_snsOptios.TopicName);

            if (createTopicResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new ApplicationException("Failed creating topic");

            _topicArn = createTopicResponse.TopicArn;
            return _topicArn;
             
        }
    }
}

