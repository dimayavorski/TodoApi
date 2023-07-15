using System;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Infrastructure.Services.Azure
{
    public class AzureBlobStorageFileService : IFileService
	{
		public AzureBlobStorageFileService()
		{
		}

        public Task DeleteFileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetFileModel> GetFileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UploadFileAsync(FileModel file)
        {
            throw new NotImplementedException();
        }
    }
}

