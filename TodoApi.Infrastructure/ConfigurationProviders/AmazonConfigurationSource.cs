using System;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Infrastructure.ConfigurationProviders
{
	public class AmazonConfigurationSource: IConfigurationSource
    {
        private readonly AmazonSecretsManagerConfig _config;
        private readonly AWSCredentials _credentials;
        public AmazonConfigurationSource(AmazonSecretsManagerConfig config, AWSCredentials credentials)
        {
            _config = config;
            _credentials = credentials;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AmazonConfigurationProvider(_config, _credentials);
        }
    }
}

