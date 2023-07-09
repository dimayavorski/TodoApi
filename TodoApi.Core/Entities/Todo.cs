using System;
namespace TodoApi.Core.Entities
{
	public class Todo
	{
		public Guid Id { get; set; }
		public required string Text { get; set; }
		public bool IsDone { get; set; }
	}
}

