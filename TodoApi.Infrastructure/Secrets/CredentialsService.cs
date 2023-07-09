using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace TodoApi.Infrastructure.Secrets
{
	public class CredentialsService: ICredentialsService
	{ 
		public AWSCredentials GetCredentials() {
			var sharedCredentialsFile = new SharedCredentialsFile();
			if(sharedCredentialsFile.TryGetProfile("TodoApi", out var profile)) {
				if(AWSCredentialsFactory.TryGetAWSCredentials(profile, sharedCredentialsFile, out var credentials)) {
					return credentials;
				}
			}
			throw new Exception("Profile Credentials could not be loaded");
		}
	}
}

