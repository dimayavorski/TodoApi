using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Commands;
using TodoApi.Application.Commands.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IMediator _mediator;
        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var getFileRequest = new GetFileQuery(id);
            var result = await _mediator.Send(getFileRequest);
            return File(result.FileStream, result.ContentType);
        }


        [HttpPost("{id:guid}")]
        public async Task<IActionResult> Post(Guid id, [FromForm]IFormFile file)
        {
            var uploadFileCommand = new UploadFileCommand(file, id);
            await _mediator.Send(uploadFileCommand);
            return Ok();
        }

        
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteFileCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}

