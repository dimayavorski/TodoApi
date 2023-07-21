namespace TodoApi.Application.Common.Interfaces
{
    public interface IMessagePublisherFactory
	{
        IMessagePublisherService GetMessagePublisher();
	}
}

