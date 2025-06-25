using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnType("INT");
            builder.Property(p => p.DataPedido).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.VendedorId).HasColumnType("INT").IsRequired();
            builder.Property(p => p.DocumentoCliente).HasMaxLength(100).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.EmailCliente).HasMaxLength(100).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.StatusPedidoId).HasColumnType("INT").IsRequired();

            builder.HasOne(p => p.Vendedor)
                .WithMany(u => u.PedidosRealizados)
                .HasForeignKey(p => p.VendedorId);

            builder.HasOne(p => p.StatusPedido)
                .WithMany()
                .HasForeignKey(p => p.StatusPedidoId);

            builder.HasMany(p => p.PedidoProduto)
                .WithOne(pp => pp.Pedido)
                .HasForeignKey(pp => pp.PedidoId);
        }
    }
}
