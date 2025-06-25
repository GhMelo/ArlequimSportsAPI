using Application.Inputs.EsporteModalidadeInput;
using Application.Inputs.PedidoInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IService
{
    internal interface IPedidoService
    {
        IEnumerable<PedidoDto> ObterTodosPedidoDto();
        IEnumerable<PedidoDto> ObterTodosPedidoPorFuncionarioIdDto(int funcionarioId);
        PedidoDto ObterPedidoDtoPorId(int id);
        void CadastrarPedido(PedidoCadastroInput pedidoCadastroInput, string nomeUsuarioLogado);
        void AlterarPedido(PedidoAlteracaoInput pedidoAlteracaoInput);
        void DeletarPedido(int id);
    }
}
