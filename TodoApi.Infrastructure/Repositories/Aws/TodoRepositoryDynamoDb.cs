using System;
using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Core.Entities;

namespace TodoApi.Infrastructure.Repositories.Aws
{
    public class TodoRepositoryDynamoDb : IRepository
    {
        private readonly IAmazonDynamoDB _amazonDybamoDb;
        private readonly string _tableName = "todoApi";
        public TodoRepositoryDynamoDb(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDybamoDb = amazonDynamoDB;
        }

        public async Task AddTodoAsync(Todo todo)
        {
            var todoAsJson = JsonSerializer.Serialize(todo);
            var todoAsAttributes = Document.FromJson(todoAsJson).ToAttributeMap();
            var createItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = todoAsAttributes
            };

            var result = await _amazonDybamoDb.PutItemAsync(createItemRequest);

            if (result.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException("Failed to create todo");
            }


        }

        public async Task DeleteAsync(Todo todo)
        {
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = todo.Id.ToString()} },
                    { "sk", new AttributeValue { S = todo.Id.ToString()} }
                }
            };

            var result = await _amazonDybamoDb.DeleteItemAsync(deleteItemRequest);

            if (result.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException("Failed to create todo");
            }


        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            var scanRequest = new ScanRequest
            {
                TableName = _tableName
            };

            var result = await _amazonDybamoDb.ScanAsync(scanRequest);

            var todos = result.Items.Select(item =>
            {
                var json = Document.FromAttributeMap(item).ToJson()!;
                return JsonSerializer.Deserialize<Todo>(json)!;
            });

            return todos;
        }

        public async Task<Todo> GetByIdAsync(Guid id)
        {

            var getItemRequest = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = id.ToString()} },
                    { "sk", new AttributeValue { S = id.ToString()} }
                }
            };

            var result = await _amazonDybamoDb.GetItemAsync(getItemRequest);

            if (!result.Item.Any())
                throw new ApplicationException("There is no todo with specified id");
            var itemAsDocumnet = Document.FromAttributeMap(result.Item);

            return JsonSerializer.Deserialize<Todo>(itemAsDocumnet.ToJson())!;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(Todo todo)
        {
            await AddTodoAsync(todo);
        }
    }
}

