using System;
using AutoMapper;
using TodoApi.Application.Commands.Handlers;
using TodoApi.Application.Common.Messages;
using TodoApi.Application.Common.Models;
using TodoApi.Application.Queries;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Common.MappingProfiles
{
	public class TodoProfile: Profile
	{
		public TodoProfile()
		{
			CreateMap<CreateTodoCommand, Todo>();
            CreateMap<UpdateTodoCommand, Todo>();
            CreateMap<Todo, TodoModel>();
			CreateMap<Todo, TodoCreatedMessage>();
		}
	}
}

