using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnType("INT");
            builder.Property(p => p.Nome).HasMaxLength(100).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.Descricao).HasMaxLength(255).HasColumnType("VARCHAR(255)").IsRequired();
            builder.Property(p => p.Preco).HasColumnType("FLOAT").IsRequired();
            builder.Property(p => p.EsporteModalidadeId).HasColumnType("INT").IsRequired();

            builder.HasOne(p => p.EsporteModalidade)
                .WithMany()
                .HasForeignKey(p => p.EsporteModalidadeId);

            builder.HasMany(p => p.ProdutoEstoque)
                .WithOne(pe => pe.Produto)
                .HasForeignKey(pe => pe.ProdutoId);

            builder.HasMany(p => p.PedidoProduto)
                .WithOne(pp => pp.Produto)
                .HasForeignKey(pp => pp.ProdutoId);
        }
    }
}
