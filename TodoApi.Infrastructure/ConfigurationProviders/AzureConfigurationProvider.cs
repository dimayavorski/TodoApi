using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace TodoApi.Infrastructure.ConfigurationProviders
{
    public class AzureConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            var secret = GetSecret();


            Data = JsonSerializer.Deserialize<Dictionary<string, string>>(secret);
        }


        private string GetSecret()
        {
            try
            {

            string secretValue = "tcC8Q~_QDXxFP45RuTeFO9XmzgT9V0B~akZIFdj-";
            string secretName = "ConnectionString";
            var tenantId = "e4a79edc-d458-416d-910e-2eefb1925dac";
            var clientId = "fe456e3a-5ce3-4bd0-b11f-9ddf1a16d541";
            var keyVaultName = "todoapi-kv";
            var kvUri = $"https://todoapi-kv.vault.azure.net/";
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, secretValue);
                var credentials = new DefaultAzureCredential();

                var client = new SecretClient(new Uri(kvUri), clientSecretCredential);

            var result = client.GetSecretAsync(secretName).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {

            }

            return string.Empty;
        }
    }
}
