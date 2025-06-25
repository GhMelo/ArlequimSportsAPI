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
        public IActionResult Get()
        {
            try
            {
                var todosProdutoDto = _produtoService.ObterTodosProdutoDto();
                return Ok(todosProdutoDto);
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
                var produtoDto = _produtoService.ObterProdutoPorId(id);
                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/ProdutosPorNome/{nome}")]
        public ActionResult GetProdutosPorNome([FromRoute] string nome)
        {
            try
            {
                var produtos = _produtoService.ObterProdutoPorNome(nome);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoCadastroInput produto)
        {
            try
            {
                _produtoService.CadastrarProduto(produto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ProdutoAlteracaoInput produto)
        {
            try
            {
                _produtoService.AlterarProduto(produto);
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
                _produtoService.DeletarProduto(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/ProdutosPorEsporteModalidade/{esporteModalidadeId:int}")]
        public IActionResult GetProdutosPorEsporteModalidade([FromRoute] int esporteModalidadeId)
        {
            try
            {
                var produtos = _produtoService.ObterProdutoPorEsporteModalidade(esporteModalidadeId);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
