using System;
using TodoApi.Infrastructure.Options;

namespace TodoApi.Infrastructure.MessageConsumer
{
	public interface IMessageConsumerFactory
	{
		public IMessageConsumer CreateConsumer(AppSettings appSettings);
	}
}

