using System;
namespace TodoApi.Application.Common.Interfaces
{
	public interface IMessageConsumerService
	{
		Task Connect();
		Task ConsumeMessage(CancellationToken cancellationToken);
	}
}

