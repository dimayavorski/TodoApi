using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace TodoApi.Application.Queries
{
    public class CreateTodoCommand: IRequest<Result<Guid>>
	{
        public required string Text { get; set; }
        public bool IsDone { get; set; }
      
    }

    public class UploadFileCommand
    {
        public IFormFile? File { get; set; }
        public string? Name { get; set; }

        public string? FileName { get; set; }
    }
}

