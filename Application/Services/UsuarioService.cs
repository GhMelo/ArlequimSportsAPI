using Application.DTOs;
using Application.Inputs.UsuarioInput;
using Application.Interfaces.IService;
using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public void AlterarUsuario(UsuarioAlteracaoInput UsuarioAlteracaoInput)
        {
            var usuarioExistente = _usuarioRepository.ObterPorId(UsuarioAlteracaoInput.Id);
            if (usuarioExistente == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            usuarioExistente.Nome = UsuarioAlteracaoInput.Nome;
            usuarioExistente.Email = UsuarioAlteracaoInput.Email;
            usuarioExistente.Senha = UsuarioAlteracaoInput.Senha;
            usuarioExistente.Tipo = UsuarioAlteracaoInput.Tipo;
            _usuarioRepository.Alterar(usuarioExistente);
        }

        public void CadastrarUsuario(UsuarioCadastroInput UsuarioCadastroInput)
        {
            _usuarioRepository.Cadastrar(new Usuario
            {
                Nome = UsuarioCadastroInput.Nome,
                Email = UsuarioCadastroInput.Email,
                Senha = UsuarioCadastroInput.Senha,
                Tipo = UsuarioCadastroInput.Tipo
            });
        }

        public void DeletarUsuario(int id)
        {
            var usuarioExistente = _usuarioRepository.ObterPorId(id);
            if (usuarioExistente == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            if (usuarioExistente.PedidosRealizados.Count() > 0 || usuarioExistente.ProdutoEstoqueMovimentacao.Count() > 0)
            {
                throw new Exception("Usuário não pode ser removido, pois está vinculado a pedidos ou movimentações de estoque.");
            }
            _usuarioRepository.Deletar(id);
        }

        public IEnumerable<UsuarioDto> ObterTodosUsuariosDto()
        {
            var usuarios = _usuarioRepository.ObterTodos();
            if (usuarios == null || !usuarios.Any())
            {
                throw new Exception("Nenhum usuário encontrado.");
            }
            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Senha = u.Senha,
                Tipo = u.Tipo,
                pedidosRealizados = u.PedidosRealizados.Select(p => new PedidoDto
                {
                    EmailCliente = p.EmailCliente,
                    DataPedido = p.DataPedido,
                    DocumentoCliente = p.DocumentoCliente,
                    StatusPedido = new StatusPedidoDto
                    {
                        Id = p.StatusPedido.Id,
                        Descricao = p.StatusPedido.Descricao
                    },
                    produtoPedido = p.PedidoProduto.Select(pp => new PedidoProdutoDto
                    {
                        ProdutoId = pp.ProdutoId,
                        PedidoId = pp.PedidoId,
                        ProdutoEstoqueId = pp.ProdutoEstoqueId,
                        Quantidade = pp.Quantidade
                    }).ToList()
                }).ToList()
            });
        }

        public UsuarioDto ObterUsuarioDtoPorId(int id)
        {
            var usuario = _usuarioRepository.ObterPorId(id);
            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Tipo = usuario.Tipo,
                pedidosRealizados = usuario.PedidosRealizados.Select(p => new PedidoDto
                {
                    EmailCliente = p.EmailCliente,
                    DataPedido = p.DataPedido,
                    DocumentoCliente = p.DocumentoCliente,
                    StatusPedido = new StatusPedidoDto
                    {
                        Id = p.StatusPedido.Id,
                        Descricao = p.StatusPedido.Descricao
                    },
                    produtoPedido = p.PedidoProduto.Select(pp => new PedidoProdutoDto
                    {
                        ProdutoId = pp.ProdutoId,
                        PedidoId = pp.PedidoId,
                        ProdutoEstoqueId = pp.ProdutoEstoqueId,
                        Quantidade = pp.Quantidade
                    }).ToList()
                }).ToList()
            };
        }

        public UsuarioDto ObterUsuarioDtoPorEmail(string email)
        {
            var usuario = _usuarioRepository.ObterTodos().Where(x=>x.Nome == email).FirstOrDefault();
            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Tipo = usuario.Tipo,
                pedidosRealizados = usuario.PedidosRealizados.Select(p => new PedidoDto
                {
                    EmailCliente = p.EmailCliente,
                    DataPedido = p.DataPedido,
                    DocumentoCliente = p.DocumentoCliente,
                    StatusPedido = new StatusPedidoDto
                    {
                        Id = p.StatusPedido.Id,
                        Descricao = p.StatusPedido.Descricao
                    },
                    produtoPedido = p.PedidoProduto.Select(pp => new PedidoProdutoDto
                    {
                        ProdutoId = pp.ProdutoId,
                        PedidoId = pp.PedidoId,
                        ProdutoEstoqueId = pp.ProdutoEstoqueId,
                        Quantidade = pp.Quantidade
                    }).ToList()
                }).ToList()
            };
        }
    }
}
