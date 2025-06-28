using Application.Inputs.TipoOperacaoInput;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class TipoOperacaoController : ControllerBase
    {
        private readonly ITipoOperacaoService _tipoOperacaoService;
        public TipoOperacaoController(ITipoOperacaoService tipoOperacaoService)
        {
            _tipoOperacaoService = tipoOperacaoService;
        }

        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public IActionResult Get()
        {
                var tiposOperacao = _tipoOperacaoService.ObterTodosTipoOperacaoDto();
                return Ok(tiposOperacao);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetById([FromRoute] int id)
        {
                var tipoOperacao = _tipoOperacaoService.ObterTipoOperacaoPorId(id);
                return Ok(tipoOperacao);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Post([FromBody] TipoOperacaoInputCadastro tipoOperacao)
        {
                _tipoOperacaoService.CadastrarTipoOperacao(tipoOperacao);
                return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] TipoOperacaoInputAlteracao tipoOperacao)
        {
                _tipoOperacaoService.AlterarTipoOperacao(tipoOperacao);
                return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
                _tipoOperacaoService.DeletarTipoOperacao(id);
                return Ok();
        }
    }
    }
