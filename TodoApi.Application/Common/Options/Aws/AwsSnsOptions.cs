using System;
namespace TodoApi.Application.Common.Options.AWS
{
	public class AwsSnsOptions
	{
		public required string TopicName { get; set; }
		public required string QueueName { get; set; }
	}
}

