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
                var todosEsporteModalidadeDto = _esporteModalidadeService.ObterTodosEsporteModalidadeDto();
                return Ok(todosEsporteModalidadeDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetById([FromRoute] int id)
        {
                var todosEsporteModalidadeDto = _esporteModalidadeService.ObterEsporteModalidadeDtoPorId(id);
                return Ok(todosEsporteModalidadeDto);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Post([FromBody] EsporteModalidadeCadastroInput input)
        {
                _esporteModalidadeService.CadastrarEsporteModalidade(input);
                return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] EsporteModalidadeAlteracaoInput input)
        {
                _esporteModalidadeService.AlterarEsporteModalidade(input);
                return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
                _esporteModalidadeService.DeletarEsporteModalidade(id);
                return Ok();
        }
    }
}
