using System;
using AutoMapper;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Commands.Handlers
{
    public record UpdateTodoCommand(Guid Id, string Text, bool IsDone) : IRequest<Guid>;
    public class UpdateTodoCommandHandler: IRequestHandler<UpdateTodoCommand, Guid>
	{
        private readonly IMapper _mapper;

        private readonly IRepositoryFactory _repositoryFactory;

        public UpdateTodoCommandHandler(IMapper mapper, IRepositoryFactory repositoryFactory)
        {
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
        }

        public async Task<Guid> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = _mapper.Map<Todo>(request);

            var repository = _repositoryFactory.GetRepository();
            await repository.UpdateAsync(todo);

            //TODO send notification to SNS
            return todo.Id;
        }
    }
}

