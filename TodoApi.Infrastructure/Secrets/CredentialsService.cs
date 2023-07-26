using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace TodoApi.Infrastructure.Secrets
{
    public class CredentialsService : ICredentialsService
    {
        public AWSCredentials GetCredentials(string? profileName = null)
        {
            var sharedCredentialsFile = new SharedCredentialsFile();
            if (!string.IsNullOrEmpty(profileName))
            {
                if (sharedCredentialsFile.TryGetProfile(profileName, out var profileSecrets))
                {
                    if (AWSCredentialsFactory.TryGetAWSCredentials(profileSecrets, sharedCredentialsFile, out var credentials))
                    {
                        return credentials;
                    }
                }
            }
            else
            { 
                if (sharedCredentialsFile.TryGetProfile("default", out var profile))
                {
                    if (AWSCredentialsFactory.TryGetAWSCredentials(profile, sharedCredentialsFile, out var credentials))
                    {
                        return credentials;
                    }
                }
            }
            throw new Exception("Profile Credentials could not be loaded");
        }
    }
}

