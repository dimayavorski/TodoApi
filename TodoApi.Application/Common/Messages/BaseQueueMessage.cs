using System;
using TodoApi.Application.Common.Interfaces;

namespace TodoApi.Application.Common.Models
{
	public class BaseQueueMessage: IQueueMessage
	{
		public BaseQueueMessage()
		{
		}

        public string MessageType { get; init; }
    }
}

