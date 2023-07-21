using System;
using MediatR;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Models;

namespace TodoApi.Application.Commands.Handlers
{
    public record GetFileQuery(Guid Id) : IRequest<GetFileModel>;

    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, GetFileModel>
	{
        private readonly IFileServiceFactory _fileServiceFactory;
		public GetFileQueryHandler(IFileServiceFactory fileServiceFactory)
		{
            _fileServiceFactory = fileServiceFactory;
		}

        public async Task<GetFileModel> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var fileService = _fileServiceFactory.GetFileService();
            var result = await fileService.GetFileAsync(request.Id);
            return result;
        }
    }
}

