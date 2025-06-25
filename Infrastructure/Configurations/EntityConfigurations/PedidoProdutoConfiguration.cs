using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class PedidoProdutoConfiguration : IEntityTypeConfiguration<PedidoProduto>
    {
        public void Configure(EntityTypeBuilder<PedidoProduto> builder)
        {
            builder.ToTable("PedidoProduto");
            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.Id).HasColumnType("INT");
            builder.Property(pp => pp.PedidoId).HasColumnType("INT").IsRequired();
            builder.Property(pp => pp.ProdutoId).HasColumnType("INT").IsRequired();
            builder.Property(pp => pp.Quantidade).HasColumnType("INT").IsRequired();

            builder.HasOne(pp => pp.Pedido)
                .WithMany()
                .HasForeignKey(pp => pp.PedidoId);

            builder.HasOne(pp => pp.Produto)
                .WithMany()
                .HasForeignKey(pp => pp.ProdutoId);
        }
    }
}
