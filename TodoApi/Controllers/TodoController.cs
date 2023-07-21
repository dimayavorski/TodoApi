using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Commands.Handlers;
using TodoApi.Application.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var getAllTodosQuery = new GetAllTodosQuery();
            return Ok(await _mediator.Send(getAllTodosQuery));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var getTodoQuery = new GetTodoQuery(id);
            var todo = await _mediator.Send(getTodoQuery);
            return Ok(todo);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateTodoCommand createTodoCommand)
        {
            var result = await _mediator.Send(createTodoCommand);

            return Ok(result);

        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateTodoCommand updateTodoCommand)
        {
           var result = await _mediator.Send(updateTodoCommand);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteTodoCommand = new DeleteTodoCommand(id);

            await _mediator.Send(deleteTodoCommand);

            return Ok();
        }
    }
}

