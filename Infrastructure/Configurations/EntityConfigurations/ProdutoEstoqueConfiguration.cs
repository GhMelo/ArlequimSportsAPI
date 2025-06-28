using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class ProdutoEstoqueConfiguration : IEntityTypeConfiguration<ProdutoEstoque>
    {
        public void Configure(EntityTypeBuilder<ProdutoEstoque> builder)
        {
            builder.ToTable("ProdutoEstoque");
            builder.HasKey(pe => pe.Id);

            builder.Property(pe => pe.Id).HasColumnType("INT");
            builder.Property(pe => pe.ProdutoId).HasColumnType("INT").IsRequired();
            builder.Property(pe => pe.NotaFiscal).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(pe => pe.Quantidade).HasColumnType("INT").IsRequired();
            builder.Property(pe => pe.DataEntrada).HasColumnType("DATETIME").IsRequired();

            builder.HasOne(pe => pe.Produto)
                .WithMany(p => p.ProdutoEstoque)
                .HasForeignKey(pe => pe.ProdutoId);
        }
    }
}
