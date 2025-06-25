using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class StatusPedidoConfiguration : IEntityTypeConfiguration<StatusPedido>
    {
        public void Configure(EntityTypeBuilder<StatusPedido> builder)
        {
            builder.ToTable("StatusPedido");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnType("INT");
            builder.Property(e => e.Descricao).IsRequired().HasMaxLength(50).HasColumnType("VARCHAR(50)");
        }
    }
}
