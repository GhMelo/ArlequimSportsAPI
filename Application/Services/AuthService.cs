using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Inputs.AuthInput;
using Application.Interfaces.IService;
using Domain.Entity;
using Domain.Interfaces.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public string FazerLogin(UsuarioLoginInput usuario)
        {
            var usuarioLogin = _usuarioRepository.obterPorEmail(usuario.Email);

            if (usuarioLogin != null
                && (usuario.Email == usuarioLogin.Email && usuario.Senha == usuarioLogin.Senha))
            {
                if (usuarioLogin.Tipo == TipoUsuario.Administrador)
                {
                    var token = GenerateToken(usuario.Email, "Administrador");
                    return token;
                }
                else
                {
                    var token = GenerateToken(usuario.Email, "Vendedor");
                    return token;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        private string GenerateToken(string username, string role)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
