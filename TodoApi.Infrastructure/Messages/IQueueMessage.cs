using System;
using MediatR;

namespace TodoApi.Infrastructure.Messages
{
	public interface IQueueMessage: IRequest
	{
		string MessageType { get; init; }
	}
}

