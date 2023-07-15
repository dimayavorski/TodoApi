namespace TodoApi.Application.Common.Models
{
	public record FileModel(Guid Id, Stream FileStream, string ContentType, string FileName, string Extension);

	public record GetFileModel(Stream FileStream, string ContentType);
}

