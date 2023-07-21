using System;
namespace TodoApi.Application.Common.Interfaces

{
	public interface IMessagePublisherService
	{
        Task PublishMessage<T>(T message) where T : IQueueMessage;
	}
}

