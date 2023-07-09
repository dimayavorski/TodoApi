using System;
using FluentResults;
using MediatR;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Queries
{
	public class CreateTodoCommand: IRequest<Result<Guid>>
	{
        public required string Text { get; set; }
        public bool IsDone { get; set; }
    }
}

