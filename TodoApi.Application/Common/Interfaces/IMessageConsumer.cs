using System;
namespace TodoApi.Application.Common.Interfaces
{
	public interface IMessageConsumerService
	{
		Task ConsumeMessage(CancellationToken cancellationToken);
	}
}

