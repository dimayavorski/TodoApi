using System;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Infrastructure.ConfigurationProviders
{
	public class AmazonConfigurationSource: IConfigurationSource
    {
        public AmazonConfigurationSource()
        {
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AmazonConfigurationProvider();
        }
    }
}

