using Microsoft.AspNetCore.Mvc;
using Application.Inputs.AuthInput;
using Application.Interfaces.IService;

namespace ArlequimSportsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioLoginInput usuario)
        {
            var usuarioLoginToken = _authService.FazerLogin(usuario);

            if (usuarioLoginToken != string.Empty)
            {
                return Ok(usuarioLoginToken);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
