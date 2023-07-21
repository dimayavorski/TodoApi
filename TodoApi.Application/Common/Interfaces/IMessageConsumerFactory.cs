using TodoApi.Application.Common.Options;

namespace TodoApi.Application.Common.Interfaces
{
	public interface IMessageConsumerFactory
	{
		public IMessageConsumerService CreateConsumer(AppSettings appSettings);
	}
}

