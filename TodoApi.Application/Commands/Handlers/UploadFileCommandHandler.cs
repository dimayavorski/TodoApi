using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Application.Commands
{
    public record UploadFileCommand(IFormFile File, Guid Id) : IRequest;
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand>
    {
        private readonly IFileServiceFactory _fileServiceFactory;
        public UploadFileCommandHandler(IFileServiceFactory fileServiceFactory)
        {
            _fileServiceFactory = fileServiceFactory;
        }

        public async Task Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var fileService = _fileServiceFactory.GetFileService();
            FileModel fileModel = new(request.Id, request.File.OpenReadStream(),
                request.File.ContentType,
                Path.GetExtension(request.File.FileName), request.File.FileName);

            await fileService.UploadFileAsync(fileModel);
        }
    }
}

