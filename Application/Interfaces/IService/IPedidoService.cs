using Application.DTOs;
using Application.Inputs.PedidoInput;

namespace Application.Interfaces.IService
{
    internal interface IPedidoService
    {
        IEnumerable<PedidoDto> ObterTodosPedidoDto();
        IEnumerable<PedidoDto> ObterTodosPedidoPorUsuarioIdDto(int usuarioId);
        PedidoDto ObterPedidoDtoPorId(int id);
        void CadastrarPedido(PedidoCadastroInput pedidoCadastroInput, string emailUsuarioLogado);
        void AlterarPedido(PedidoAlteracaoInput pedidoAlteracaoInput, string emailUsuarioLogado);
        void DeletarPedido(int id, string emailUsuarioLogado);
    }
}
