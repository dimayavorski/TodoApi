using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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

        public async Task DeleteFileAsync(Guid id)
        {
            await _blobContainerClient.DeleteBlobIfExistsAsync(id.ToString());
        }

        public async Task<GetFileModel> GetFileAsync(Guid id)
        {
            var blobClient = _blobContainerClient.GetBlobClient(id.ToString());
            var result = await blobClient.DownloadContentAsync();
            return new GetFileModel(result.Value.Content.ToStream(), result.Value.Details.ContentType);
        }

        public async Task UploadFileAsync(FileModel file)
        {
            var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };
            var blobClient = _blobContainerClient.GetBlobClient(file.Id.ToString());
            await blobClient.UploadAsync(file.FileStream, blobHttpHeader);

        }
    }
}

