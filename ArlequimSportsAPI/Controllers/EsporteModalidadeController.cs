using Application.Inputs.EsporteModalidadeInput;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class EsporteModalidadeController : ControllerBase
    {
        private readonly IEsporteModalidadeService _esporteModalidadeService;

        public EsporteModalidadeController(IEsporteModalidadeService esporteModalidadeService)
        {
            _esporteModalidadeService = esporteModalidadeService;
        }

        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public IActionResult Get()
        {
            try
            {
                var todosJogosDto = _esporteModalidadeService.ObterTodosEsporteModalidadeDto();
                return Ok(todosJogosDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var todosJogosDto = _esporteModalidadeService.ObterEsporteModalidadeDtoPorId(id);
                return Ok(todosJogosDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Post([FromBody] EsporteModalidadeCadastroInput input)
        {
            try
            {
                _esporteModalidadeService.CadastrarEsporteModalidade(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] EsporteModalidadeAlteracaoInput input)
        {
            try
            {
                _esporteModalidadeService.AlterarEsporteModalidade(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _esporteModalidadeService.DeletarEsporteModalidade(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
