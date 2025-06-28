using Application.Inputs.ProdutoInput;
using Application.Interfaces.IService;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimSportsAPI.Controllers
{

    [ApiController]
    [Route("/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        [Authorize(Policy = "Vendedor")]
        public IActionResult Get()
        {
                var todosProdutoDto = _produtoService.ObterTodosProdutoDto();
                return Ok(todosProdutoDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetById([FromRoute] int id)
        {
                var produtoDto = _produtoService.ObterProdutoPorId(id);
                return Ok(produtoDto);
        }

        [HttpGet("/ProdutosPorNome/{nome}")]
        [Authorize(Policy = "Administrador")]
        public ActionResult GetProdutosPorNome([FromRoute] string nome)
        {
                var produtos = _produtoService.ObterProdutoPorNome(nome);
                return Ok(produtos);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Post([FromBody] ProdutoCadastroInput produto)
        {
                _produtoService.CadastrarProduto(produto);
                return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] ProdutoAlteracaoInput produto)
        {
                _produtoService.AlterarProduto(produto);
                return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
                _produtoService.DeletarProduto(id);
                return Ok();
        }

        [HttpGet("/ProdutosPorEsporteModalidade/{esporteModalidadeId:int}")]
        [Authorize(Policy = "Vendedor")]
        public IActionResult GetProdutosPorEsporteModalidade([FromRoute] int esporteModalidadeId)
        {
                var produtos = _produtoService.ObterProdutoPorEsporteModalidade(esporteModalidadeId);
                return Ok(produtos);
        }
    }
}
