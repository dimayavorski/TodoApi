using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Infrastructure.Services.Aws
{
    public class S3FileService : IFileService
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucketName = "todoapistorage-original";
        public S3FileService(IAmazonS3 s3)
        {
            _s3 = s3;

        }

        public async Task DeleteFileAsync(Guid id)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = $"images/{id}"
            };

            var result = await _s3.DeleteObjectAsync(deleteObjectRequest);

            if(result.HttpStatusCode != HttpStatusCode.NoContent)
            {
                throw new ApplicationException("Failed to delete file from s3 storage");
            }
            
            
        }

        public async Task<GetFileModel> GetFileAsync(Guid id)
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = $"images/{id}"
            };
            try
            {
                var result = await _s3.GetObjectAsync(getObjectRequest);

                if (result.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Failed to fetch file from s3 storage");
                }
                var fileModel = new GetFileModel(result.ResponseStream, result.Headers.ContentType);
                return fileModel;
            }
            catch (AmazonS3Exception ex)
            {
                throw new ApplicationException($"{ex.Message} : Id: {id}");
            }
        }

        public async Task UploadFileAsync(FileModel file)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"images/{file.Id}",
                ContentType = file.ContentType,
                InputStream = file.FileStream,
                Metadata =
                {
                    ["x-amz-meta-originalname"] = file.FileName,
                    ["x-amz-meta-extension"] = file.Extension
                }
            };
            var result = await _s3.PutObjectAsync(putObjectRequest);

            if (result.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException("Failed to upload to s3 storage");
            }
        }
    }
}

