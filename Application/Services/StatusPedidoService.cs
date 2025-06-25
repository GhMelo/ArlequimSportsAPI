using Application.DTOs;
using Application.Inputs.StatusPedidoInput;
using Application.Interfaces.IService;
using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Application.Services
{
    public class StatusPedidoService : IStatusPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IStatusPedidoRepository _statusPedidoRepository;
        public StatusPedidoService(IPedidoRepository pedidoRepository, IStatusPedidoRepository statusPedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _statusPedidoRepository = statusPedidoRepository;
        }

        public void DeletarStatusPedido(int id)
        {
            if (_pedidoRepository.ObterTodos().Where(x => x.StatusPedidoId == id).Any())
            {
                throw new Exception("Status de pedido não pode ser removido, pois está vinculado a pedidos existentes.");
            }
            else
            {
                _statusPedidoRepository.Deletar(id);
            }
        }

        public StatusPedidoDto ObterStatusPedidoPorId(int id)
        {
            return new StatusPedidoDto
            {
                Id = id,
                Descricao = _statusPedidoRepository.ObterPorId(id).Descricao
            };
        }

        public IEnumerable<StatusPedidoDto> ObterTodosStatusPedidoDto()
        {
            return _statusPedidoRepository.ObterTodos().Select(x => new StatusPedidoDto
            {
                Id = x.Id,
                Descricao = x.Descricao
            });
        }

        public void AlterarStatusPedido(StatusPedidoAlteracaoInput statusPedidoAlteracaoInput)
        {
            if (_pedidoRepository.ObterTodos().Where(x => x.StatusPedidoId == statusPedidoAlteracaoInput.Id).Any())
            {
                throw new Exception("Status de pedido não pode ser alterado, pois está vinculado a pedidos existentes.");
            }
            else
            {
                var statusPedido = new StatusPedido
                {
                    Id = statusPedidoAlteracaoInput.Id,
                    Descricao = statusPedidoAlteracaoInput.Descricao
                };
                _statusPedidoRepository.Alterar(statusPedido);
            }
        }

        public void CadastrarStatusPedido(StatusPedidoCadastroInput statusPedidoCadastroInput)
        {
            var statusPedido = new StatusPedido
            {
                Descricao = statusPedidoCadastroInput.Descricao
            };
            _statusPedidoRepository.Cadastrar(statusPedido);
        }
    }
}
