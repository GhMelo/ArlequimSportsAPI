using Application.Inputs.ProdutoEstoqueInput;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ProdutoEstoqueController : Controller
    {
        private readonly IProdutoEstoqueService _produtoEstoqueService;
        public ProdutoEstoqueController(IProdutoEstoqueService produtoEstoqueService)
        {
            _produtoEstoqueService = produtoEstoqueService;
        }

        [HttpGet]
        [Authorize(Policy = "Vendedor")]
        public IActionResult Get()
        {
                var produtosEstoque = _produtoEstoqueService.ObterTodosProdutoEstoqueDto();
                return Ok(produtosEstoque);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Vendedor")]
        public IActionResult GetById([FromRoute] int id)
        {
                var produtoEstoque = _produtoEstoqueService.ObterProdutoEstoqueDtoPorId(id);
                return Ok(produtoEstoque);
        }

        [HttpGet("/TodosProdutoEstoquePorProduto/{id:int}")]
        [Authorize(Policy = "Vendedor")]
        public IActionResult GetProdutosEstoquePorProduto([FromRoute] int id)
        {
                var produtosEstoque = _produtoEstoqueService.ObterTodosProdutoEstoquePorProdutoIdDto(id);
                return Ok(produtosEstoque);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Post([FromBody] ProdutoEstoqueCadastroInput input)
        {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _produtoEstoqueService.CadastrarProdutoEstoque(input, emailUsuarioLogado);
                return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] ProdutoEstoqueAlteracaoInput input)
        {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _produtoEstoqueService.AlterarProdutoEstoque(input, emailUsuarioLogado);
                return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _produtoEstoqueService.DeletarProdutoEstoque(id, emailUsuarioLogado);
                return Ok();
        }
    }
}
