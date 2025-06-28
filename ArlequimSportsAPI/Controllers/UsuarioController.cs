using Application.Inputs.UsuarioInput;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Administrador")]
        public IActionResult Get()
        {
                var usuariosDto = _usuarioService.ObterTodosUsuariosDto();
                return Ok(usuariosDto);
        }

        [HttpGet("/UsuarioPorId/{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult GetUsuarioPorId([FromRoute] int id)
        {
                var usuarioDto = _usuarioService.ObterUsuarioDtoPorId(id);
                return Ok(usuarioDto);
        }

        [HttpGet("GetUsuarioPorEmail/{email}")]
        [Authorize(Policy = "Administrador")]
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
                _usuarioService.CadastrarUsuario(usuarioCadastroInput);
                return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "Administrador")]
        public IActionResult Put([FromBody] UsuarioAlteracaoInput usuarioAlteracaoInput)
        {
                _usuarioService.AlterarUsuario(usuarioAlteracaoInput);
                return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete([FromRoute] int id)
        {
                _usuarioService.DeletarUsuario(id);
                return Ok();
        }
    }
}
