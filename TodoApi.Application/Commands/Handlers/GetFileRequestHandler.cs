using System;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Application.Commands.Handlers
{
    public record GetFileRequest(Guid Id) : IRequest<GetFileModel>;

    public class GetFileRequestHandler : IRequestHandler<GetFileRequest, GetFileModel>
	{
        private readonly IFileServiceFactory _fileServiceFactory;
		public GetFileRequestHandler(IFileServiceFactory fileServiceFactory)
		{
            _fileServiceFactory = fileServiceFactory;
		}

        public async Task<GetFileModel> Handle(GetFileRequest request, CancellationToken cancellationToken)
        {
            var fileService = _fileServiceFactory.GetFileService();
            var result = await fileService.GetFileAsync(request.Id);
            return result;
        }
    }
}

