using Application.Inputs.TipoOperacaoInput;
using Application.Interfaces.IService;
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
        public IActionResult Get()
        {
            try
            {
                var tiposOperacao = _tipoOperacaoService.ObterTodosTipoOperacaoDto();
                return Ok(tiposOperacao);
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
                var tipoOperacao = _tipoOperacaoService.ObterTipoOperacaoPorId(id);
                return Ok(tipoOperacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TipoOperacaoInputCadastro tipoOperacao)
        {
            try
            {
                _tipoOperacaoService.CadastrarTipoOperacao(tipoOperacao);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] TipoOperacaoInputAlteracao tipoOperacao)
        {
            try
            {
                _tipoOperacaoService.AlterarTipoOperacao(tipoOperacao);
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
                _tipoOperacaoService.DeletarTipoOperacao(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }
