namespace Application.DTOs
{
    public class MensagemPagamentoDto
    {
        public int PedidoId { get; set; }
        public string DocumentoCliente { get; set; } = null!;
    }
}
