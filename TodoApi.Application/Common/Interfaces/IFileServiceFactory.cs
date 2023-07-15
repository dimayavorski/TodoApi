using System;
using TodoApi.Application.Common.Interfaces;

namespace TodoApi.Application.Common.Interfaces
{
	public interface IFileServiceFactory
	{
		IFileService GetFileService();
	}
}

