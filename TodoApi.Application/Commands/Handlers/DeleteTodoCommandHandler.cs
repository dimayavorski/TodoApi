using System;
using AutoMapper;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Commands.Handlers
{
    public record DeleteTodoCommand(Guid Id) : IRequest;
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repositoryFactory;

        public DeleteTodoCommandHandler(IMapper mapper, IRepositoryFactory repositoryFactory)
        {
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
        }

        public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {

            var repository = _repositoryFactory.GetRepository();
            await repository.DeleteTodoAsync(request.Id);

            //TODO send notification to SNS
        }
    }
}

