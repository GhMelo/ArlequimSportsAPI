using Application.Inputs.StatusPedidoInput;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class StatusPedidoController : ControllerBase
    {
        private readonly IStatusPedidoService _statusPedidoService;
        public StatusPedidoController(IStatusPedidoService statusPedidoService)
        {
            _statusPedidoService = statusPedidoService;
        }
        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public IActionResult Get()
        {
                var statusPedidos = _statusPedidoService.ObterTodosStatusPedidoDto();
                return Ok(statusPedidos);
        }
        [HttpGet("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetById([FromRoute] int id)
        {
                var statusPedido = _statusPedidoService.ObterStatusPedidoPorId(id);
                return Ok(statusPedido);
        }
        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Post([FromBody] StatusPedidoCadastroInput statusPedido)
        {
                _statusPedidoService.CadastrarStatusPedido(statusPedido);
                return Ok();
        }
        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] StatusPedidoAlteracaoInput statusPedido)
        {
                _statusPedidoService.AlterarStatusPedido(statusPedido);
                return Ok();
        }
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
                _statusPedidoService.DeletarStatusPedido(id);
                return Ok();
        }
    }
}
