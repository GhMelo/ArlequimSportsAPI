using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IService;
using Application.Inputs.PedidoInput;
using System.Security.Claims;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var pedidos = _pedidoService.ObterTodosPedidoDto();
                return Ok(pedidos);
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
                var pedidos = _pedidoService.ObterPedidoDtoPorId(id);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/TodosPedidoPorUsuario/{id:int}")]
        public IActionResult GetPedidosPorUsuario([FromRoute] int id)
        {
            try
            {
                var pedidos = _pedidoService.ObterTodosPedidoPorUsuarioIdDto(id);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PedidoCadastroInput pedidoCadastroInput)
        {
            try
            {
                var emailUsuarioLogado = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _pedidoService.CadastrarPedido(pedidoCadastroInput, emailUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] PedidoAlteracaoInput pedidoAlteracaoInput)
        {
            try
            {
                var emailUsuarioLogado = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _pedidoService.AlterarPedido(pedidoAlteracaoInput, emailUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            try
            {
                var emailUsuarioLogado = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _pedidoService.DeletarPedido(id, emailUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
