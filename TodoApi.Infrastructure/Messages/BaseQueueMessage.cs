using System;
namespace TodoApi.Infrastructure.Messages
{
	public class BaseQueueMessage: IQueueMessage
	{
		public BaseQueueMessage()
		{
		}

        public string MessageType { get; init; }
    }
}

