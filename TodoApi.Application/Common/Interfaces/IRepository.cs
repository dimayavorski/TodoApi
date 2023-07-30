using System;
using TodoApi.Core.Entities;

namespace TodoApi.Application.Common.Interfaces
{
	public interface IRepository
	{
		Task AddTodoAsync(Todo todo);
		Task UpdateAsync(Todo todo);
		Task DeleteAsync(Todo todo);
		Task<IEnumerable<Todo>> GetAllAsync();
		Task<Todo> GetByIdAsync(Guid id);
		Task SaveAsync();
	}
}

