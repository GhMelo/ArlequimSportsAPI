using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class ProdutoEstoqueMovimentacaoConfiguration : IEntityTypeConfiguration<ProdutoEstoqueMovimentacao>
    {
        public void Configure(EntityTypeBuilder<ProdutoEstoqueMovimentacao> builder)
        {
            builder.ToTable("ProdutoEstoqueMovimentacao");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).HasColumnType("INT");
            builder.Property(m => m.ProdutoEstoqueId).HasColumnType("INT").IsRequired();
            builder.Property(m => m.TipoOperacaoId).HasColumnType("INT").IsRequired();
            builder.Property(m => m.Quantidade).HasColumnType("INT").IsRequired();
            builder.Property(m => m.IdUsuario).HasColumnType("INT").IsRequired();
            builder.Property(m => m.DataMovimentacao).HasColumnType("DATETIME").IsRequired();

            builder.HasOne(m => m.ProdutoEstoque)
                .WithMany()
                .HasForeignKey(m => m.ProdutoEstoqueId);

            builder.HasOne(m => m.TipoOperacao)
                .WithMany()
                .HasForeignKey(m => m.TipoOperacaoId);

            builder.HasOne(m => m.Usuario)
                .WithMany(u => u.ProdutoEstoqueMovimentacao)
                .HasForeignKey(m => m.IdUsuario);
        }
    }
}
