using System;
namespace TodoApi.Application.Common.Options.Aws
{
    public class SqsOptions
    {
        public required string QueueName { get; set; }
        public required string QueueUrl { get; set; }
    }

}

