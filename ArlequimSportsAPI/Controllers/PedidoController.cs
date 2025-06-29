using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IService;
using Application.Inputs.PedidoInput;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Policy = "Vendedor")]
        public IActionResult Get()
        {
                var pedidos = _pedidoService.ObterTodosPedidoDto();
                return Ok(pedidos);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Vendedor")]
        public IActionResult GetById([FromRoute] int id)
        {
                var pedidos = _pedidoService.ObterPedidoDtoPorId(id);
                return Ok(pedidos);
        }

        [HttpGet("/TodosPedidoPorUsuario/{id:int}")]
        [Authorize(Policy = "Vendedor")]
        public IActionResult GetPedidosPorUsuario([FromRoute] int id)
        {
                var pedidos = _pedidoService.ObterTodosPedidoPorUsuarioIdDto(id);
                return Ok(pedidos);
        }

        [HttpGet("/ConfirmarEmailPedido/{id:int}")]
        public IActionResult GetConfirmarEmailPedido([FromRoute] int id)
        {
             _pedidoService.ConfirmarEmailPedido(id);
            return Ok();
        }

        [HttpGet("/AlterarStatusPagamentoPedido/{statusPagamento:int}/{id:int}")]
        public IActionResult GetAlterarStatusPagamentoPedido([FromRoute] int statusPagamento, int id)
        {
            _pedidoService.AlterarStatusPagamentoPedido(statusPagamento,id);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Vendedor")]
        public IActionResult Post([FromBody] PedidoCadastroInput pedidoCadastroInput)
        {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _pedidoService.CadastrarPedido(pedidoCadastroInput, emailUsuarioLogado);
                return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] PedidoAlteracaoInput pedidoAlteracaoInput)
        {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _pedidoService.AlterarPedido(pedidoAlteracaoInput, emailUsuarioLogado);
                return Ok();
        }
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromBody] int id)
        {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _pedidoService.DeletarPedido(id, emailUsuarioLogado);
                return Ok();
        }
    }
}
