using System;
using AutoMapper;
using FluentResults;
using MediatR;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Queries
{
    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Result<Guid>>
	{
        private readonly IMapper _mapper;

        public CreateTodoCommandHandler(IMapper mapper)
		{
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = _mapper.Map<Todo>(request);
            todo.Id = Guid.NewGuid();
            //save to database
            //save image to s3
            //send notification

            return await Task.FromResult(Result.Ok(todo.Id));
        }

      
    }
}

