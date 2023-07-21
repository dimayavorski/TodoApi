using System;
using AutoMapper;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Queries
{
	public record GetAllTodosQuery(): IRequest<IEnumerable<TodoModel>>;
	public class GetAllTodosQueryHandler: IRequestHandler<GetAllTodosQuery, IEnumerable<TodoModel>>
	{
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repositoryFactory;

        public GetAllTodosQueryHandler(IMapper mapper, IRepositoryFactory repositoryFactory)
        {
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
        }

        public async Task<IEnumerable<TodoModel>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            var repository = _repositoryFactory.GetRepository();

            var todos = await repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoModel>>(todos);
        }
    }
}

