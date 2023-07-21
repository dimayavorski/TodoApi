using System;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Application.Common.Messages
{
	public class TodoCreatedMessage: IQueueMessage
	{
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }
	}
}

