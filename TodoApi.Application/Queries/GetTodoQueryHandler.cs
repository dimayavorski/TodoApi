using System;
using AutoMapper;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Application.Commands.Handlers
{
	public record GetTodoQuery(Guid Id): IRequest<TodoModel>;

    public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, TodoModel>
	{
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repositoryFactory;

        public GetTodoQueryHandler(IMapper mapper, IRepositoryFactory repositoryFactory)
        {
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
        }

        
        public async Task<TodoModel> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var repository = _repositoryFactory.GetRepository();

            var todo = await repository.GetByIdAsync(request.Id);
            var todoModel = _mapper.Map<TodoModel>(todo);
            return todoModel;
        }
    }
}

