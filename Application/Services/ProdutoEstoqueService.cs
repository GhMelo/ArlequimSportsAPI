using Application.DTOs;
using Application.Inputs.ProdutoEstoqueInput;
using Application.Interfaces.IService;
using Application.Interfaces.IUnitOfWork;
using Domain.Entity;
using Domain.Enums;
using Domain.Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProdutoEstoqueService : IProdutoEstoqueService
    {

        private readonly IProdutoEstoqueRepository _produtoEstoqueRepository;
        private readonly IProdutoEstoqueMovimentacaoRepository _produtoEstoqueMovimentacaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProdutoEstoqueService(
            IProdutoEstoqueRepository produtoEstoqueRepository,
            IProdutoEstoqueMovimentacaoRepository produtoEstoqueMovimentacaoRepository,
            IUsuarioRepository usuarioRepository,
            IProdutoRepository produtoRepository,
            IUnitOfWork unitOfWork)
        {
            _produtoEstoqueRepository = produtoEstoqueRepository;
            _produtoEstoqueMovimentacaoRepository = produtoEstoqueMovimentacaoRepository;
            _usuarioRepository = usuarioRepository;
            _produtoRepository = produtoRepository;
            _unitOfWork = unitOfWork;
        }

        public void AlterarProdutoEstoque(ProdutoEstoqueAlteracaoInput produtoEstoqueAlteracaoInput, string emailUsuarioLogado)
        {
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);

            _unitOfWork.BeginTransaction();

            try
            {
                if (_produtoEstoqueRepository.ObterPorId(produtoEstoqueAlteracaoInput.ProdutoEstoqueId) == null)
                    throw new Exception("Estoque não encontrado.");

                foreach (var produto in produtoEstoqueAlteracaoInput.Produtos)
                {
                    if (_produtoRepository.ObterPorId(produto.ProdutoId) == null)
                        throw new Exception($"Produto com ID {produto.ProdutoId} não encontrado.");

                    var produtoEstoque = _produtoEstoqueRepository.ObterTodos()
                        .FirstOrDefault(x =>
                            x.ProdutoId == produto.ProdutoId &&
                            x.Id == produtoEstoqueAlteracaoInput.ProdutoEstoqueId);

                    if (produtoEstoque == null)
                    {
                        var novoProdutoEstoque = new ProdutoEstoque
                        {
                            ProdutoId = produto.ProdutoId,
                            Quantidade = produto.Quantidade,
                            NotaFiscal = produtoEstoqueAlteracaoInput.NotaFiscal,
                            DataEntrada = produtoEstoqueAlteracaoInput.DataEntrada
                        };

                        _produtoEstoqueRepository.Cadastrar(novoProdutoEstoque);

                        var movimentacao = new ProdutoEstoqueMovimentacao
                        {
                            ProdutoEstoqueId = novoProdutoEstoque.Id,
                            Quantidade = produto.Quantidade,
                            DataMovimentacao = DateTime.Now,
                            TipoOperacaoId = (int)ETipoMovimentacao.Entrada,
                            IdUsuario = usuarioLogado.Id
                        };

                        _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                    }
                    else
                    {
                        var quantidadeAntes = produtoEstoque.Quantidade;
                        produtoEstoque.Quantidade += produto.Quantidade;

                        if (produtoEstoque.Quantidade < 0)
                            throw new Exception($"Quantidade do produto {produto.ProdutoId} não pode ser negativa.");

                        produtoEstoque.NotaFiscal = produtoEstoqueAlteracaoInput.NotaFiscal;
                        produtoEstoque.DataEntrada = produtoEstoqueAlteracaoInput.DataEntrada;

                        _produtoEstoqueRepository.Alterar(produtoEstoque);

                        var tipoMovimentacao = produto.Quantidade < 0
                            ? (int)ETipoMovimentacao.Saida
                            : (int)ETipoMovimentacao.Entrada;

                        var movimentacao = new ProdutoEstoqueMovimentacao
                        {
                            ProdutoEstoqueId = produtoEstoque.Id,
                            Quantidade = Math.Abs(produto.Quantidade),
                            DataMovimentacao = DateTime.Now,
                            TipoOperacaoId = tipoMovimentacao,
                            IdUsuario = usuarioLogado.Id
                        };

                        _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                    }
                }

                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }
        public void CadastrarProdutoEstoque(ProdutoEstoqueCadastroInput produtoEstoqueCadastroInput, string emailUsuarioLogado)
        {
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);

            _unitOfWork.BeginTransaction();

            try
            {
                foreach (var produto in produtoEstoqueCadastroInput.Produtos)
                {
                    if (_produtoRepository.ObterPorId(produto.ProdutoId) == null)
                        throw new Exception($"Produto com ID {produto.ProdutoId} não encontrado.");

                    var produtoEstoque = new ProdutoEstoque
                    {
                        ProdutoId = produto.ProdutoId,
                        Quantidade = produto.Quantidade,
                        NotaFiscal = produtoEstoqueCadastroInput.NotaFiscal,
                        DataEntrada = DateTime.Now
                    };

                    _produtoEstoqueRepository.Cadastrar(produtoEstoque);

                    var movimentacao = new ProdutoEstoqueMovimentacao
                    {
                        ProdutoEstoqueId = produtoEstoque.Id,
                        Quantidade = produto.Quantidade,
                        DataMovimentacao = DateTime.Now,
                        TipoOperacaoId = (int)ETipoMovimentacao.Entrada,
                        IdUsuario = usuarioLogado.Id
                    };

                    _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                }

                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public void DeletarProdutoEstoque(int id, string emailUsuarioLogado)
        {
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);
            var produtoEstoqueRemocao = _produtoEstoqueRepository.ObterTodos().Where(x => x.Id == id);

            if (produtoEstoqueRemocao.Count() == 0)
            {
                throw new Exception("Produto estoque não encontrado.");
            }
            if(produtoEstoqueRemocao.Any(x=>x.Quantidade > 0))
            {
                throw new Exception("Não é possível remover o estoque, pois ainda há quantidade disponível.");
            } 

            foreach(var produtoEstoque in produtoEstoqueRemocao)
            {
                _produtoEstoqueRepository.Deletar(produtoEstoque.Id);
            }
        }

        public IEnumerable<ProdutoEstoqueDto> ObterProdutoEstoqueDtoPorId(int id)
        {
            var produtoEstoqueRemocao = _produtoEstoqueRepository.ObterTodos().Where(x => x.Id == id);
            return produtoEstoqueRemocao.Select(pe => new ProdutoEstoqueDto
            {
                Quantidade = pe.Quantidade,
                NotaFiscal = pe.NotaFiscal,
                DataEntrada = pe.DataEntrada,
                Produto = new ProdutoDto
                {
                    Nome = pe.Produto.Nome,
                    Descricao = pe.Produto.Descricao,
                    Preco = pe.Produto.Preco,
                    EsporteModalidade = new EsporteModalidadeDto
                    {
                        Id = pe.Produto.EsporteModalidade.Id,
                        Descricao = pe.Produto.EsporteModalidade.Descricao
                    }
                }

            }).ToList();
        }

        public IEnumerable<ProdutoEstoqueDto> ObterTodosProdutoEstoqueDto()
        {
            var produtoEstoqueRemocao = _produtoEstoqueRepository.ObterTodos();
            return produtoEstoqueRemocao.Select(pe => new ProdutoEstoqueDto
            {
                Quantidade = pe.Quantidade,
                NotaFiscal = pe.NotaFiscal,
                DataEntrada = pe.DataEntrada,
                Produto = new ProdutoDto
                {
                    Nome = pe.Produto.Nome,
                    Descricao = pe.Produto.Descricao,
                    Preco = pe.Produto.Preco,
                    EsporteModalidade = new EsporteModalidadeDto
                    {
                        Id = pe.Produto.EsporteModalidade.Id,
                        Descricao = pe.Produto.EsporteModalidade.Descricao
                    }
                }

            }).ToList();
        }

        public IEnumerable<ProdutoEstoqueDto> ObterTodosProdutoEstoquePorProdutoIdDto(int produtoId)
        {
            var produtoEstoqueRemocao = _produtoEstoqueRepository.ObterTodos().Where(x => x.ProdutoId == produtoId);
            return produtoEstoqueRemocao.Select(pe => new ProdutoEstoqueDto
            {
                Quantidade = pe.Quantidade,
                NotaFiscal = pe.NotaFiscal,
                DataEntrada = pe.DataEntrada,
                Produto = new ProdutoDto
                {
                    Nome = pe.Produto.Nome,
                    Descricao = pe.Produto.Descricao,
                    Preco = pe.Produto.Preco,
                    EsporteModalidade = new EsporteModalidadeDto
                    {
                        Id = pe.Produto.EsporteModalidade.Id,
                        Descricao = pe.Produto.EsporteModalidade.Descricao
                    }
                }

            }).ToList();
        }
    }
}
