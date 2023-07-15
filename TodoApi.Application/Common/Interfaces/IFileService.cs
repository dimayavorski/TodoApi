using System;
using FluentResults;
using TodoApi.Application.Commands;
using TodoApi.Application.Common.Models;

namespace TodoApi.Application.Common.Interfaces
{
	public interface IFileService
	{
		Task UploadFileAsync(FileModel file);
		Task<GetFileModel> GetFileAsync(Guid id);
		Task DeleteFileAsync(Guid id);
	}
}

