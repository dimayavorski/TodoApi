using System;
using Amazon.DynamoDBv2;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Core.Entities;

namespace TodoApi.Infrastructure.Repositories.Azure
{
    public class TodoRepositoryCosmosDb : IRepository
	{
		private readonly IAmazonDynamoDB _amazonDybamoDb;
		public TodoRepositoryCosmosDb(IAmazonDynamoDB amazonDynamoDB)
		{
			_amazonDybamoDb = amazonDynamoDB;
		}

        public Task AddTodoAsync(Todo todo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTodoAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Todo todo)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Todo>> IRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Todo> IRepository.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

