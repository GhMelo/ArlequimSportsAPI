using Application.Validations.DataAnnotations;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Application.Inputs.UsuarioInput
{
    public class UsuarioCadastroInput
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [SenhaAttribute(TamanhoMinimo = 8)]
        public required string Senha { get; set; }

        [Required(ErrorMessage = "Tipo é obrigatório.")]
        public required TipoUsuario Tipo { get; set; }
    }
}
