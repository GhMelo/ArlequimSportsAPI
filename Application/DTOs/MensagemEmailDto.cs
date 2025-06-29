namespace Application.DTOs
{
    public class MensagemEmailDto
    {
        public string Para { get; set; } = null!;
        public string Assunto { get; set; } = null!;
        public string Corpo { get; set; } = null!;
    }
}
