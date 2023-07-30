using System;
using AutoMapper;
using FluentResults;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Messages;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Queries
{
    public record CreateTodoCommand(string Text, bool IsDone) : IRequest<Guid>;

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid>
    {
        private readonly IMapper _mapper;

        private readonly IRepositoryFactory _repositoryFactory;

        private readonly IMessagePublisherFactory _messagePublisherFactory;

        public CreateTodoCommandHandler(IMapper mapper, IRepositoryFactory repositoryFactory, IMessagePublisherFactory messagePublisherFactory)
        {
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
            _messagePublisherFactory = messagePublisherFactory;
        }

        public async Task<Guid> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = _mapper.Map<Todo>(request);
            todo.Id = Guid.NewGuid();

            var repository = _repositoryFactory.GetRepository();
            await repository.AddTodoAsync(todo);
            await repository.SaveAsync();

            //var messagePublisherService = _messagePublisherFactory.GetMessagePublisher();
            //var message = _mapper.Map<TodoCreatedMessage>(todo);
            //message.CreatedAt = DateTime.UtcNow;
            //await messagePublisherService.PublishMessage<TodoCreatedMessage>(message);
            
            return todo.Id;
        }


    }
}

