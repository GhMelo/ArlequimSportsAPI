using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EntityConfigurations
{
    public class TipoOperacaoConfiguration : IEntityTypeConfiguration<TipoOperacao>
    {
        public void Configure(EntityTypeBuilder<TipoOperacao> builder)
        {
            builder.ToTable("TipoOperacao");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnType("INT");
            builder.Property(e => e.Descricao).IsRequired().HasMaxLength(50).HasColumnType("VARCHAR(50)");
        }
    }
}
