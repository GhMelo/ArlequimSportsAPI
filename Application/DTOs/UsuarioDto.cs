using Domain.Entity;

namespace Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public TipoUsuario Tipo { get; set; }
        public ICollection<PedidoDto> pedidosRealizados { get; set; } = new List<PedidoDto>();
    }
}
