using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Validations.DataAnnotations
{
    public class SenhaAttribute : ValidationAttribute
    {
        public int TamanhoMinimo { get; set; } = 8;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var senha = value as string;

            if (string.IsNullOrWhiteSpace(senha))
                return new ValidationResult("A senha é obrigatória.");

            if (senha.Length < TamanhoMinimo)
                return new ValidationResult($"A senha deve ter pelo menos {TamanhoMinimo} caracteres.");

            if (!Regex.IsMatch(senha, @"[A-Z]"))
                return new ValidationResult("A senha deve conter ao menos uma letra maiúscula.");

            if (!Regex.IsMatch(senha, @"[a-z]"))
                return new ValidationResult("A senha deve conter ao menos uma letra minúscula.");

            if (!Regex.IsMatch(senha, @"[0-9]"))
                return new ValidationResult("A senha deve conter ao menos um número.");

            if (!Regex.IsMatch(senha, @"[\W_]"))
                return new ValidationResult("A senha deve conter ao menos um caractere especial.");

            return ValidationResult.Success;
        }
    }
}
