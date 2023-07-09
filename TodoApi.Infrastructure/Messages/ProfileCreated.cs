using System;
using MediatR;

namespace TodoApi.Infrastructure.Messages
{
    public class ProfileCreated : BaseQueueMessage
    {
        public ProfileCreated()
        {
        }

        public required Guid Id { get; init; }

        public required string GitHubUsername { get; init; }

        public required string FullName { get; init; }

        public required string Email { get; init; }
    }
}

