using System;
using System.Text.Json.Serialization;

namespace TodoApi.Core.Entities
{
	public class Todo
	{
		[JsonPropertyName("pk")]
		public string Pk => Id.ToString();
		[JsonPropertyName("sk")]
		public string Sk => Id.ToString();

		public Guid Id { get; set; }
		public required string Text { get; set; }
		public bool IsDone { get; set; }
	}
}

