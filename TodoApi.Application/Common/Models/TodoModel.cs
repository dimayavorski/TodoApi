using System;
namespace TodoApi.Application.Common.Models
{
	public class TodoModel
	{
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public bool IsDone { get; set; }
    }
}

