using Application.Inputs.UsuarioInput;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var usuariosDto = _usuarioService.ObterTodosUsuariosDto();
                return Ok(usuariosDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/UsuarioPorId/{id:int}")]
        public IActionResult GetUsuarioPorId([FromRoute] int id)
        {
            try
            {
                var usuarioDto = _usuarioService.ObterUsuarioDtoPorId(id);
                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsuarioPorEmail/{email}")]
        public IActionResult GetUsuarioPorEmail([FromRoute] string email)
        {
            var usuario = _usuarioService.ObterUsuarioDtoPorEmail(email);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UsuarioCadastroInput usuarioCadastroInput)
        {
            try
            {
                _usuarioService.CadastrarUsuario(usuarioCadastroInput);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UsuarioAlteracaoInput usuarioAlteracaoInput)
        {
            try
            {
                _usuarioService.AlterarUsuario(usuarioAlteracaoInput);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _usuarioService.DeletarUsuario(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
