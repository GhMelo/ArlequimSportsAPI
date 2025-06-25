using Application.Inputs.StatusPedidoInput;
using Application.Interfaces.IService;
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
        public IActionResult Get()
        {
            try
            {
                var statusPedidos = _statusPedidoService.ObterTodosStatusPedidoDto();
                return Ok(statusPedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var statusPedido = _statusPedidoService.ObterStatusPedidoPorId(id);
                return Ok(statusPedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] StatusPedidoCadastroInput statusPedido)
        {
            try
            {
                _statusPedidoService.CadastrarStatusPedido(statusPedido);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public IActionResult Put([FromBody] StatusPedidoAlteracaoInput statusPedido)
        {
            try
            {
                _statusPedidoService.AlterarStatusPedido(statusPedido);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _statusPedidoService.DeletarStatusPedido(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
