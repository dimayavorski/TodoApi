using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Core.Entities;

namespace TodoApi.Infrastructure.Repositories.Azure
{
    public class TodoRepositoryCosmosDb : IRepository
	{
		private readonly ApplicationContenxt _applicationContext;
        private readonly DbSet<Todo> _table;
		public TodoRepositoryCosmosDb(ApplicationContenxt applicationContext)
		{
			_applicationContext = applicationContext;
            _table = _applicationContext.Set<Todo>();
		}

        public async Task AddTodoAsync(Todo todo)
        {
            await _table.AddAsync(todo);
        }

        public async Task DeleteAsync(Todo todo)
        {
            await Task.FromResult(_table.Remove(todo));
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<Todo> GetByIdAsync(Guid id)
        {
            return await _table.FirstAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Todo todo)
        {
            await Task.FromResult(_table.Update(todo));
        }

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }


    }
}

