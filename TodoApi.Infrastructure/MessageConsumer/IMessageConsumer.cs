using System;
namespace TodoApi.Infrastructure.MessageConsumer
{
	public interface IMessageConsumer
	{
		Task Connect();
		Task ConsumeMessage(CancellationToken cancellationToken);
	}
}

