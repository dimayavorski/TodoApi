using System;
namespace TodoApi.Application.Common.Options.Azure
{
	public class AzureServiceBusOptions
	{
		public const string SectionName = "AzureServiceBus";

		public string ConnectionString { get; set; } = default!;
		public string TopicName { get; set; } = default!;
        public string SubscriberName { get; set; } = default!;	
    }
}

