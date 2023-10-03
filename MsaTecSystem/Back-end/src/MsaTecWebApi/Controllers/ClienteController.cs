using MediatR;
using Microsoft.AspNetCore.Mvc;
using MsaTec.Application.Mediators.Commands;
using MsaTec.Application.Mediators.Queries;
using MsaTec.Application.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MsaTec.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IMediator _Mediator;

    public ClienteController(IMediator mediator)
    {
        _Mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetAllClientesQuery();
        var clientes = await _Mediator.Send(query);

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new GetClienteByIdQuery(id);
        var cliente = await _Mediator.Send(query);

        if (cliente != null)
        {
            return Ok(cliente);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCliente([FromBody] ClienteViewModel clienteViewModel)
    {
        var command = new InsertClienteCommand(clienteViewModel);
        var result = await _Mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Get), new { id = result.Result }, clienteViewModel);
        }

        return BadRequest(result.MessageError);
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateCliente([FromBody] ClienteViewModel clienteViewModel)
    {
        var command = new UpdateClienteCommand(clienteViewModel);
        var result = await _Mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Get), new { id = result.Result }, clienteViewModel);
        }

        return BadRequest(result.MessageError);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteClienteCommand(id);
        var result = await _Mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok($"Cliente excluido: {id}");
        }

        return BadRequest(result.MessageError);
    }
}
