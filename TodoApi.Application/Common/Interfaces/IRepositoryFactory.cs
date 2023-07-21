using System;
namespace TodoApi.Application.Common.Interfaces
{
	public interface IRepositoryFactory
	{
		IRepository GetRepository();
	}
}

