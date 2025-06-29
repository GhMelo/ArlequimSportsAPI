using System.Net.Mail;
using Application.DTOs;
using Application.Inputs.PedidoInput;
using Application.Interfaces.IProducer;
using Application.Interfaces.IService;
using Application.Interfaces.IUnitOfWork;
using Domain.Entity;
using Domain.Enums;
using Domain.Interfaces.IRepository;

namespace Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoProdutoRepository _pedidoProdutoRepository;
        private readonly IProdutoEstoqueRepository _produtoEstoqueRepository;
        private readonly IProdutoEstoqueMovimentacaoRepository _produtoEstoqueMovimentacaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailKafkaProducer _emailProducer;

        public PedidoService(IPedidoRepository pedidoRepository, 
            IPedidoProdutoRepository pedidoProdutoRepository, 
            IProdutoEstoqueRepository produtoEstoqueRepository,
            IProdutoEstoqueMovimentacaoRepository produtoEstoqueMovimentacaoRepository,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork,
            IEmailKafkaProducer emailKafkaProducer) 
        {
            _pedidoRepository = pedidoRepository;
            _pedidoProdutoRepository = pedidoProdutoRepository;
            _produtoEstoqueRepository = produtoEstoqueRepository;
            _produtoEstoqueMovimentacaoRepository = produtoEstoqueMovimentacaoRepository;
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
            _emailProducer = emailKafkaProducer;
        }
        public void AlterarPedido(PedidoAlteracaoInput pedidoAlteracaoInput, string emailUsuarioLogado)
        {
            _unitOfWork.BeginTransaction();

            try
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
                        if (pedidoProduto.Quantidade != produtoAlteraca.Quantidade)
                        {
                            var produtoEstoque = _produtoEstoqueRepository.ObterPorId(pedidoProduto.ProdutoEstoqueId);
                            var quantidadeDiferenca = produtoAlteraca.Quantidade - pedidoProduto.Quantidade;

                            if (quantidadeDiferenca > 0)
                            {
                                if (produtoEstoque.Quantidade < quantidadeDiferenca)
                                {
                                    throw new Exception("Quantidade insuficiente em estoque para realizar a alteração do pedido.");
                                }
                                produtoEstoque.Quantidade -= quantidadeDiferenca;
                            }
                            else
                            {
                                produtoEstoque.Quantidade += Math.Abs(quantidadeDiferenca);
                            }

                            var movimentacao = new ProdutoEstoqueMovimentacao
                            {
                                ProdutoEstoqueId = produtoEstoque.Id,
                                Quantidade = Math.Abs(quantidadeDiferenca),
                                TipoOperacaoId = quantidadeDiferenca > 0
                                    ? (int)ETipoMovimentacao.Saida
                                    : (int)ETipoMovimentacao.Entrada,
                                IdUsuario = usuarioLogado.Id
                            };

                            _produtoEstoqueRepository.Alterar(produtoEstoque);
                            _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);

                            pedidoProduto.Quantidade = produtoAlteraca.Quantidade;
                            _pedidoProdutoRepository.Alterar(pedidoProduto);
                        }
                    }
                    else
                    {
                        var produtoEstoque = _produtoEstoqueRepository.ObterTodos()
                            .FirstOrDefault(p =>
                                p.ProdutoId == produtoAlteraca.ProdutoId &&
                                p.Quantidade >= produtoAlteraca.Quantidade);

                        if (produtoEstoque == null)
                        {
                            throw new Exception("Quantidade insuficiente em estoque para realizar a alteração do pedido.");
                        }

                        var novoPedidoProduto = new PedidoProduto
                        {
                            PedidoId = pedidoAlteracaoInput.PedidoId,
                            ProdutoId = produtoAlteraca.ProdutoId,
                            Quantidade = produtoAlteraca.Quantidade,
                            ProdutoEstoqueId = produtoEstoque.Id
                        };

                        produtoEstoque.Quantidade -= produtoAlteraca.Quantidade;

                        var movimentacao = new ProdutoEstoqueMovimentacao
                        {
                            ProdutoEstoqueId = produtoEstoque.Id,
                            Quantidade = produtoAlteraca.Quantidade,
                            TipoOperacaoId = (int)ETipoMovimentacao.Saida,
                            IdUsuario = usuarioLogado.Id
                        };

                        _produtoEstoqueRepository.Alterar(produtoEstoque);
                        _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                        _pedidoProdutoRepository.Cadastrar(novoPedidoProduto);
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


        public void CadastrarPedido(PedidoCadastroInput pedidoCadastroInput, string emailUsuarioLogado)
        {
            var usuarioLogado = _usuarioRepository.obterPorEmail(emailUsuarioLogado);

            _unitOfWork.BeginTransaction();

            try
            {
                var pedido = new Pedido
                {
                    DocumentoCliente = pedidoCadastroInput.DocumentoCliente,
                    EmailCliente = pedidoCadastroInput.EmailCliente,
                    VendedorId = usuarioLogado.Id,
                    StatusPedidoId = (int)EStatusPedido.AguardandoConfirmacaoEmail
                };

                _pedidoRepository.Cadastrar(pedido);

                foreach (var produtoCadastro in pedidoCadastroInput.Produtos)
                {
                    var produtoEstoque = _produtoEstoqueRepository.ObterTodos()
                        .FirstOrDefault(p =>
                            p.ProdutoId == produtoCadastro.ProdutoId &&
                            p.Quantidade >= produtoCadastro.Quantidade);

                    if (produtoEstoque == null)
                        throw new Exception("Quantidade insuficiente em estoque para realizar a alteração do pedido.");

                    var novoPedidoProduto = new PedidoProduto
                    {
                        PedidoId = pedido.Id,
                        ProdutoId = produtoCadastro.ProdutoId,
                        Quantidade = produtoCadastro.Quantidade,
                        ProdutoEstoqueId = produtoEstoque.Id
                    };

                    produtoEstoque.Quantidade -= produtoCadastro.Quantidade;

                    var movimentacao = new ProdutoEstoqueMovimentacao
                    {
                        ProdutoEstoqueId = produtoEstoque.Id,
                        Quantidade = Math.Abs(produtoCadastro.Quantidade),
                        TipoOperacaoId = (int)ETipoMovimentacao.Saida,
                        IdUsuario = usuarioLogado.Id
                    };

                    _produtoEstoqueRepository.Alterar(produtoEstoque);
                    _produtoEstoqueMovimentacaoRepository.Cadastrar(movimentacao);
                    _pedidoProdutoRepository.Cadastrar(novoPedidoProduto);
                }

                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();

                var mensagem = new MensagemEmailDto
                {
                    Para = pedido.EmailCliente,
                    Assunto = "Pedido Recebido",
                    Corpo = $"Olá! Recebemos seu pedido nº {pedido.Id}. " +
                            $"Para confirmar seu pedido clique no link a seguir https://localhost:7261/ConfirmarEmailPedido/{pedido.Id}"
                };

                _emailProducer.EnviarMensagemAsync(mensagem); 
            }
            catch
            {
                _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public void ConfirmarEmailPedido(int id)
        {
            var pedido = _pedidoRepository.ObterPorId(id);
            pedido.StatusPedidoId = (int)EStatusPedido.EmailConfirmado;
            _pedidoRepository.Alterar(pedido);
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
