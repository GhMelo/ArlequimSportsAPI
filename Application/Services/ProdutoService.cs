using Application.DTOs;
using Application.Inputs.ProdutoInput;
using Application.Interfaces.IService;
using Domain.Entity;
using Domain.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
        public void AlterarProduto(ProdutoAlteracaoInput produtoAlteracaoInput)
        {
            var produtoAlteracao = _produtoRepository.ObterPorId(produtoAlteracaoInput.ProdutoId);
            if (produtoAlteracao == null)
            {
                throw new Exception("Produto não encontrado.");
            }
            produtoAlteracao.Nome = produtoAlteracaoInput.Nome;
            produtoAlteracao.Descricao = produtoAlteracaoInput.Descricao;
            produtoAlteracao.Preco = produtoAlteracaoInput.Preco;
            produtoAlteracao.EsporteModalidadeId = produtoAlteracaoInput.EsporteModalidadeId;
            _produtoRepository.Alterar(produtoAlteracao);

        }

        public void CadastrarProduto(ProdutoCadastroInput produtoCadastroInput)
        {
            var novoProduto = new Produto
            {
                Nome = produtoCadastroInput.Nome,
                Descricao = produtoCadastroInput.Descricao,
                Preco = produtoCadastroInput.Preco,
                EsporteModalidadeId = produtoCadastroInput.EsporteModalidadeId
            };
            _produtoRepository.Cadastrar(novoProduto);
        }

        public void DeletarProduto(int id)
        {
            var produtoRemocao = _produtoRepository.ObterPorId(id);
            if (produtoRemocao.ProdutoEstoque.Count() > 0 || produtoRemocao.PedidoProduto.Count() > 0)
            {
                throw new Exception("Produto não pode ser removido, pois está vinculado a estoque ou pedidos.");
            }
            else
            {
                _produtoRepository.Deletar(id);
            }
        }

        public IEnumerable<ProdutoDto> ObterProdutoPorEsporteModalidade(int esporteModalidadeId)
        {
            var produtoBanco = _produtoRepository.ObterTodos().Where(x=>x.EsporteModalidadeId == esporteModalidadeId);
            return produtoBanco.Select(p => new ProdutoDto
            {
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                EsporteModalidade = new EsporteModalidadeDto
                {
                    Id = p.EsporteModalidade.Id,
                    Descricao = p.EsporteModalidade.Descricao
                }
            }).ToList();
        }

        public ProdutoDto ObterProdutoPorId(int id)
        {
            var produtoBanco = _produtoRepository.ObterPorId(id);
            return new ProdutoDto
            {
                Nome = produtoBanco.Nome,
                Descricao = produtoBanco.Descricao,
                Preco = produtoBanco.Preco,
                EsporteModalidade = new EsporteModalidadeDto
                {
                    Id = produtoBanco.EsporteModalidade.Id,
                    Descricao = produtoBanco.EsporteModalidade.Descricao
                }
            };
        }

        public IEnumerable<ProdutoDto> ObterProdutoPorNome(string nome)
        {
            var produtoBanco = _produtoRepository.ObterTodos().Where(x => x.Nome == nome);
            return produtoBanco.Select(p => new ProdutoDto
            {
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                EsporteModalidade = new EsporteModalidadeDto
                {
                    Id = p.EsporteModalidade.Id,
                    Descricao = p.EsporteModalidade.Descricao
                }
            }).ToList();
        }

        public IEnumerable<ProdutoDto> ObterTodosProdutoDto()
        {
            var produtoBanco = _produtoRepository.ObterTodos();
            return produtoBanco.Select(p => new ProdutoDto
            {
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                EsporteModalidade = new EsporteModalidadeDto
                {
                    Id = p.EsporteModalidade.Id,
                    Descricao = p.EsporteModalidade.Descricao
                }
            }).ToList();
        }
    }
}
