using System;
using MediatR;
using TodoApi.Application.Common.Interfaces;

namespace TodoApi.Application.Commands.Handlers
{
    //public record DeleteFileCommand(Guid Id) : IRequest;
    public class DeleteFileCommand: IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
	{
        private readonly IFileServiceFactory _fileServiceFactory;
        public DeleteFileCommandHandler(IFileServiceFactory fileServiceFactory)
		{
            _fileServiceFactory = fileServiceFactory;
		}

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var fileService = _fileServiceFactory.GetFileService();
            await fileService.DeleteFileAsync(request.Id);
        }
    }
}

