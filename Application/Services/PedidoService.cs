using Application.DTOs;
using Application.Inputs.PedidoInput;
using Application.Interfaces.IService;
using Domain.Entity;
using Domain.Enums;
using Domain.Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoProdutoRepository _pedidoProdutoRepository;
        private readonly IProdutoEstoqueRepository _produtoEstoqueRepository;
        private readonly IProdutoEstoqueMovimentacaoRepository _produtoEstoqueMovimentacaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly DbContext _context;
        public PedidoService(IPedidoRepository pedidoRepository, 
            IPedidoProdutoRepository pedidoProdutoRepository, 
            IProdutoEstoqueRepository produtoEstoqueRepository,
            IProdutoEstoqueMovimentacaoRepository produtoEstoqueMovimentacaoRepository,
            IUsuarioRepository usuarioRepository,
            DbContext dbContext) 
        {
            _pedidoRepository = pedidoRepository;
            _pedidoProdutoRepository = pedidoProdutoRepository;
            _produtoEstoqueRepository = produtoEstoqueRepository;
            _produtoEstoqueMovimentacaoRepository = produtoEstoqueMovimentacaoRepository;
            _usuarioRepository = usuarioRepository;
            _context = dbContext;
        }
        public void AlterarPedido(PedidoAlteracaoInput pedidoAlteracaoInput, string emailUsuarioLogado)
        {
            var pedido = _pedidoRepository.ObterPorId(pedidoAlteracaoInput.PedidoId);
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);
            pedido.StatusPedidoId = pedidoAlteracaoInput.StatusPedidoId;
            pedido.DocumentoCliente = pedidoAlteracaoInput.DocumentoCliente;
            pedido.EmailCliente = pedidoAlteracaoInput.EmailCliente;
            pedido.VendedorId = pedidoAlteracaoInput.VendedorId;

            _pedidoRepository.Alterar(pedido);

            var pedidoProdutosDb = _pedidoProdutoRepository.obterTodosPorPedidoId(pedidoAlteracaoInput.PedidoId);

            foreach (var produtoAlteraca in pedidoAlteracaoInput.Produtos)
            {
                var pedidoProduto = pedidoProdutosDb.FirstOrDefault(pp => pp.ProdutoId == produtoAlteraca.ProdutoId);
                if (pedidoProduto != null)
                {
                    if(pedidoProduto.Quantidade != produtoAlteraca.Quantidade)
                    {
                        var produtoEstoque = _produtoEstoqueRepository.ObterPorId(pedidoProduto.ProdutoEstoqueId);

                        var quantidadeDiferenca = produtoAlteraca.Quantidade - pedidoProduto.Quantidade;

                        if (quantidadeDiferenca > 0)
                        {
                            if (produtoEstoque.Quantidade < quantidadeDiferenca)
                            {
                                throw new Exception("Quantidade insuficiente em estoque para realizar a alteração do pedido.");
                            }
                            else{
                                produtoEstoque.Quantidade = produtoEstoque.Quantidade - quantidadeDiferenca;
                            }
                        }
                        else
                        {
                            produtoEstoque.Quantidade = produtoEstoque.Quantidade + Math.Abs(quantidadeDiferenca);
                        }

                        var movimentacao = new ProdutoEstoqueMovimentacao()
                        {
                            ProdutoEstoqueId = produtoEstoque.Id,
                            Quantidade = Math.Abs(quantidadeDiferenca),
                            TipoOperacaoId = quantidadeDiferenca > 0 ? ((int)ETipoMovimentacao.Entrada) : ((int)ETipoMovimentacao.Saida),
                            IdUsuario = usuarioLogado.Id
                        };

                        _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                        _produtoEstoqueRepository.Alterar(produtoEstoque);

                        pedidoProduto.Quantidade = produtoAlteraca.Quantidade;
                        _pedidoProdutoRepository.Alterar(pedidoProduto);
                    }
                }
                else
                {
                    var produtoEstoque = _produtoEstoqueRepository.ObterTodos().Where(p => p.ProdutoId == produtoAlteraca.ProdutoId && p.Quantidade >= produtoAlteraca.Quantidade).FirstOrDefault();

                    if (produtoEstoque != null)
                    {
                        var novoPedidoProduto = new PedidoProduto
                        {
                            PedidoId = pedidoAlteracaoInput.PedidoId,
                            ProdutoId = produtoAlteraca.ProdutoId,
                            Quantidade = produtoAlteraca.Quantidade,
                            ProdutoEstoqueId = produtoEstoque.Id
                        };

                        produtoEstoque.Quantidade = produtoEstoque.Quantidade - produtoAlteraca.Quantidade;

                        var movimentacao = new ProdutoEstoqueMovimentacao()
                        {
                            ProdutoEstoqueId = produtoEstoque.Id,
                            Quantidade = Math.Abs(produtoAlteraca.Quantidade),
                            TipoOperacaoId = ((int)ETipoMovimentacao.Saida),
                            IdUsuario = usuarioLogado.Id
                        };

                        _produtoEstoqueRepository.Alterar(produtoEstoque);
                        _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                        _pedidoProdutoRepository.Cadastrar(novoPedidoProduto);
                    }
                    else
                    {
                        throw new Exception("Quantidade insuficiente em estoque para realizar a alteração do pedido.");
                    }
                }
            }
        }

        public void CadastrarPedido(PedidoCadastroInput pedidoCadastroInput, string emailUsuarioLogado)
        {
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var pedido = new Pedido
                    {
                        DocumentoCliente = pedidoCadastroInput.DocumentoCliente,
                        EmailCliente = pedidoCadastroInput.EmailCliente,
                        VendedorId = usuarioLogado.Id,
                        StatusPedidoId = (int)EStatusPedido.Aberto
                    };
                    _pedidoRepository.Cadastrar(pedido);

                    foreach (var produtoCadastro in pedidoCadastroInput.Produtos)
                    {
                        var produtoEstoque = _produtoEstoqueRepository.ObterTodos()
                            .Where(p => p.ProdutoId == produtoCadastro.ProdutoId && p.Quantidade >= produtoCadastro.Quantidade)
                            .FirstOrDefault();

                        if (produtoEstoque != null)
                        {
                            var novoPedidoProduto = new PedidoProduto
                            {
                                PedidoId = pedido.Id,
                                ProdutoId = produtoCadastro.ProdutoId,
                                Quantidade = produtoCadastro.Quantidade,
                                ProdutoEstoqueId = produtoEstoque.Id
                            };

                            produtoEstoque.Quantidade = produtoEstoque.Quantidade - produtoCadastro.Quantidade;

                            var movimentacao = new ProdutoEstoqueMovimentacao()
                            {
                                ProdutoEstoqueId = produtoEstoque.Id,
                                Quantidade = Math.Abs(produtoCadastro.Quantidade),
                                TipoOperacaoId = ((int)ETipoMovimentacao.Saida),
                                IdUsuario = usuarioLogado.Id
                            };

                            _produtoEstoqueRepository.Alterar(produtoEstoque);
                            _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                            _pedidoProdutoRepository.Cadastrar(novoPedidoProduto);
                        }
                        else
                        {
                            throw new Exception("Quantidade insuficiente em estoque para realizar a alteração do pedido.");
                        }
                    }

                    _context.SaveChanges(); 
                    transaction.Commit();   
                }
                catch
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public void DeletarPedido(int id, string emailUsuarioLogado)
        {
            var pedidoProdutos = _pedidoProdutoRepository.obterTodosPorPedidoId(id);
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);

            foreach (var pedidoProduto in pedidoProdutos)
            {
                var produtoEstoque = _produtoEstoqueRepository.ObterPorId(pedidoProduto.ProdutoEstoqueId);
                produtoEstoque.Quantidade += pedidoProduto.Quantidade;
                var movimentacao = new ProdutoEstoqueMovimentacao()
                {
                    ProdutoEstoqueId = produtoEstoque.Id,
                    Quantidade = pedidoProduto.Quantidade,
                    TipoOperacaoId = (int)ETipoMovimentacao.Entrada,
                    IdUsuario = usuarioLogado.Id
                };
                _produtoEstoqueRepository.Alterar(produtoEstoque);
                _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
            }

            _pedidoRepository.Deletar(id);
        }

        public PedidoDto? ObterPedidoDtoPorId(int id)
        {
            var pedidoDb = _pedidoRepository.ObterPorId(id);
            return new PedidoDto
            {
                DocumentoCliente = pedidoDb.DocumentoCliente,
                EmailCliente = pedidoDb.EmailCliente,
                Vendedor = new UsuarioDto()
                {
                    Id = pedidoDb.Vendedor.Id,
                    Nome = pedidoDb.Vendedor.Nome,
                    Email = pedidoDb.Vendedor.Email,
                },
                StatusPedido = new StatusPedidoDto()
                {
                    Id = pedidoDb.StatusPedido.Id,
                    Descricao = pedidoDb.StatusPedido.Descricao
                },
                produtoPedido = _pedidoProdutoRepository.obterTodosPorPedidoId(id)
                    .Select(pp => new PedidoProdutoDto
                    {
                        ProdutoId = pp.ProdutoId,
                        Quantidade = pp.Quantidade,
                        ProdutoEstoqueId = pp.ProdutoEstoqueId
                    }).ToList(),
            };
        }

        public IEnumerable<PedidoDto> ObterTodosPedidoDto()
        {
            var pedidoDb = _pedidoRepository.ObterTodos();
            foreach(var pedido in pedidoDb)
            {
                yield return new PedidoDto
                {
                    DocumentoCliente = pedido.DocumentoCliente,
                    EmailCliente = pedido.EmailCliente,
                    Vendedor = new UsuarioDto()
                    {
                        Id = pedido.Vendedor.Id,
                        Nome = pedido.Vendedor.Nome,
                        Email = pedido.Vendedor.Email,
                    },
                    StatusPedido = new StatusPedidoDto()
                    {
                        Id = pedido.StatusPedido.Id,
                        Descricao = pedido.StatusPedido.Descricao
                    },
                    produtoPedido = _pedidoProdutoRepository.obterTodosPorPedidoId(pedido.Id)
                        .Select(pp => new PedidoProdutoDto
                        {
                            ProdutoId = pp.ProdutoId,
                            Quantidade = pp.Quantidade,
                            ProdutoEstoqueId = pp.ProdutoEstoqueId
                        }).ToList(),
                };
            }
        }

        public IEnumerable<PedidoDto> ObterTodosPedidoPorUsuarioIdDto(int usuarioId)
        {
            var pedidoDb = _pedidoRepository.ObterTodos().Where(x=>x.VendedorId == usuarioId);
            foreach (var pedido in pedidoDb)
            {
                yield return new PedidoDto
                {
                    DocumentoCliente = pedido.DocumentoCliente,
                    EmailCliente = pedido.EmailCliente,
                    Vendedor = new UsuarioDto()
                    {
                        Id = pedido.Vendedor.Id,
                        Nome = pedido.Vendedor.Nome,
                        Email = pedido.Vendedor.Email,
                    },
                    StatusPedido = new StatusPedidoDto()
                    {
                        Id = pedido.StatusPedido.Id,
                        Descricao = pedido.StatusPedido.Descricao
                    },
                    produtoPedido = _pedidoProdutoRepository.obterTodosPorPedidoId(pedido.Id)
                        .Select(pp => new PedidoProdutoDto
                        {
                            ProdutoId = pp.ProdutoId,
                            Quantidade = pp.Quantidade,
                            ProdutoEstoqueId = pp.ProdutoEstoqueId
                        }).ToList(),
                };
            }
        }
    }
}
