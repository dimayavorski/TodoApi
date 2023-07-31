using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System;
using System.Reflection.Metadata;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;
using TodoApi.Application.Common.Options.Azure;

namespace TodoApi.Infrastructure.Services.Azure
{
    public class AzureBlobStorageFileService : IFileService
	{
        private readonly AzureBlobStorageOptions _azureBlobStorageOptions;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
		public AzureBlobStorageFileService(IOptions<AzureBlobStorageOptions> azureBlobStorageOptions, BlobServiceClient blobServiceClient)
		{
            _azureBlobStorageOptions = azureBlobStorageOptions.Value ?? throw new ArgumentNullException(nameof(azureBlobStorageOptions));
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(_azureBlobStorageOptions.ContainerName);

		}

        public Task DeleteFileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetFileModel> GetFileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UploadFileAsync(FileModel file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(file.Id.ToString());

            await using (Stream data = blobClient.OpenRead())
            {
                await blobClient.UploadAsync(data);
            }

        }
    }
}

