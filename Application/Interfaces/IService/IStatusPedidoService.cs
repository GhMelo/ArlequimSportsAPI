using Application.Inputs.StatusPedidoInput;

namespace Application.Interfaces.IService
{
    public interface IStatusPedidoService
    {
        IEnumerable<StatusPedidoDto> ObterTodosStatusPedidoDto();
        StatusPedidoDto ObterStatusPedidoPorId(int id);
        void CadastrarStatusPedido(StatusPedidoCadastroInput statusPedidoCadastroInput);
        void AlterarStatusPedido(StatusPedidoAlteracaoInput statusPedidoAlteracaoInput);
        void DeletarStatusPedido(int id);
    }
}
