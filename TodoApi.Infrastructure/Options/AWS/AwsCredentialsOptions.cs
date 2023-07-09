using System;
namespace TodoApi.Infrastructure.Options.AWS
{
	public class AwsCredentialsOptions
	{
        public required string AccessKey { get; set; }
        public required string AwsRegion { get; set; }
        public required string Secret { get; set; }
    }
}

