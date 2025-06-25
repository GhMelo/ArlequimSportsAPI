using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class EsporteModalidadeConfiguration : IEntityTypeConfiguration<EsporteModalidade>
    {
        public void Configure(EntityTypeBuilder<EsporteModalidade> builder)
        {
            builder.ToTable("EsporteModalidade");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnType("INT");
            builder.Property(e => e.Descricao).IsRequired().HasMaxLength(100).HasColumnType("VARCHAR(100)");
        }
    }
}