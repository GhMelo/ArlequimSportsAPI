using Application.Inputs.ProdutoEstoqueInput;
using Application.Interfaces.IService;
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
        public IActionResult Get()
        {
            try
            {
                var produtosEstoque = _produtoEstoqueService.ObterTodosProdutoEstoqueDto();
                return Ok(produtosEstoque);
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
                var produtoEstoque = _produtoEstoqueService.ObterProdutoEstoqueDtoPorId(id);
                return Ok(produtoEstoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/TodosProdutoEstoquePorProduto/{id:int}")]
        public IActionResult GetProdutosEstoquePorProduto([FromRoute] int id)
        {
            try
            {
                var produtosEstoque = _produtoEstoqueService.ObterTodosProdutoEstoquePorProdutoIdDto(id);
                return Ok(produtosEstoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoEstoqueCadastroInput input)
        {
            try
            {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _produtoEstoqueService.CadastrarProdutoEstoque(input, emailUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ProdutoEstoqueAlteracaoInput input)
        {
            try
            {
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _produtoEstoqueService.AlterarProdutoEstoque(input, emailUsuarioLogado);
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
                var emailUsuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _produtoEstoqueService.DeletarProdutoEstoque(id, emailUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
