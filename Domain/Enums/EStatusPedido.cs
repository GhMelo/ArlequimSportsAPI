namespace Domain.Enums
{
    public enum EStatusPedido
    {
        Aberto = 1,
        AguardandoConfirmacaoEmail = 2,
        EmailConfirmado = 3,
        AguardandoPagamento = 4,
        PagamentoConfirmado = 5,
        PedidoEmProducao = 6,
        Finalizado = 7,
        Cancelado = 8,
    }
}
