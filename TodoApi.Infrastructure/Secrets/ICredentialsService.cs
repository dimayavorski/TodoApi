using Amazon.Runtime;

namespace TodoApi.Infrastructure.Secrets
{
    public interface ICredentialsService
	{
		AWSCredentials GetCredentials(string? profileName = null);
	}
}

