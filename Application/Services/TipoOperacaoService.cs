using Application.DTOs;
using Application.Inputs.TipoOperacaoInput;
using Application.Interfaces.IService;
using Domain.Interfaces.IRepository;

namespace Application.Services
{
    public class TipoOperacaoService : ITipoOperacaoService
    {
        private readonly IProdutoEstoqueMovimentacaoRepository _produtoEstoqueMovimentacaoRepository;
        private readonly ITipoOperacaoRepository _tipoOperacaoRepository;

        public TipoOperacaoService(IProdutoEstoqueMovimentacaoRepository produtoEstoqueMovimentacaoRepository, ITipoOperacaoRepository tipoOperacaoRepository)
        {
            _produtoEstoqueMovimentacaoRepository = produtoEstoqueMovimentacaoRepository;
            _tipoOperacaoRepository = tipoOperacaoRepository;
        }

        public void AlterarTipoOperacao(TipoOperacaoInputAlteracao tipoOperacaoAlteracaoInput)
        {
            if (_produtoEstoqueMovimentacaoRepository.ObterTodos().Any(x => x.TipoOperacaoId == tipoOperacaoAlteracaoInput.Id))
            {
                throw new Exception("Tipo de operação não pode ser alterado, pois está vinculado a movimentações de estoque existentes.");
            }
            else
            {
                var tipoOperacao = new Domain.Entity.TipoOperacao
                {
                    Id = tipoOperacaoAlteracaoInput.Id,
                    Descricao = tipoOperacaoAlteracaoInput.Descricao
                };
                _tipoOperacaoRepository.Alterar(tipoOperacao);
            }
        }

        public void CadastrarTipoOperacao(TipoOperacaoInputCadastro tipoOperacaoCadastroInput)
        {
            var tipoOperacao = new Domain.Entity.TipoOperacao
            {
                Descricao = tipoOperacaoCadastroInput.Descricao
            };
            _tipoOperacaoRepository.Cadastrar(tipoOperacao);
        }

        public void DeletarTipoOperacao(int id)
        {
            if (_produtoEstoqueMovimentacaoRepository.ObterTodos().Any(x => x.TipoOperacaoId == id))
            {
                throw new Exception("Tipo de operação não pode ser removido, pois está vinculado a movimentações de estoque existentes.");
            }
            else
            {
                _tipoOperacaoRepository.Deletar(id);
            }
        }

        public TipoOperacaoDto ObterTipoOperacaoPorId(int id)
        {
            var tipoOperacao = _tipoOperacaoRepository.ObterPorId(id);
            if (tipoOperacao == null)
            {
                throw new Exception("Tipo de operação não encontrado.");
            }
            return new TipoOperacaoDto
            {
                Id = tipoOperacao.Id,
                Descricao = tipoOperacao.Descricao
            };
        }

        public IEnumerable<TipoOperacaoDto> ObterTodosTipoOperacaoDto()
        {
            return _tipoOperacaoRepository.ObterTodos().Select(x => new TipoOperacaoDto
            {
                Id = x.Id,
                Descricao = x.Descricao
            }).ToList();
        }
    }
}
