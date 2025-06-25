using Application.DTOs;
using Application.Inputs.StatusPedidoInput;

namespace Application.Interfaces.IService
{
    public interface IStatusPedidoService
    {
        public IEnumerable<StatusPedidoDto> ObterTodosStatusPedidoDto();
        public StatusPedidoDto ObterStatusPedidoPorId(int id);
        void CadastrarStatusPedido(StatusPedidoCadastroInput statusPedidoCadastroInput);
        void AlterarStatusPedido(StatusPedidoAlteracaoInput statusPedidoAlteracaoInput);
        void DeletarStatusPedido(int id);
    }
}
