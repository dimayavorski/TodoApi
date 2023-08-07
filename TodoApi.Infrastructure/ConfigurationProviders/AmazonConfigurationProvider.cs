using System;
using System.Text.Json;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Infrastructure.ConfigurationProviders
{
	public class AmazonConfigurationProvider: ConfigurationProvider
	{
		public AmazonConfigurationProvider()
		{
			
		}

        public override void Load()
        {
            var secret = GetSecret();
            

            Data = JsonSerializer.Deserialize<Dictionary<string, string>>(secret);
        }


        private string GetSecret()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}",
            };

            using (var client = new AmazonSecretsManagerClient(region: Amazon.RegionEndpoint.EUWest2))
            {
                var response = client.GetSecretValueAsync(request).GetAwaiter().GetResult();

                string secretString;
                if (response.SecretString != null)
                {
                    secretString = response.SecretString;
                }
                else
                {
                    var memoryStream = response.SecretBinary;
                    var reader = new StreamReader(memoryStream);
                    secretString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                }

                return secretString;
            }
        }
    }
}

