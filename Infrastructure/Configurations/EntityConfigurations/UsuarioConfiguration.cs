using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(u => u.Id);

            builder.ToTable("Usuario", b =>
            {
                b.HasCheckConstraint("CK_Usuario_SenhaMinima", "LEN([Senha]) >= 8");
                b.HasCheckConstraint("CK_Usuario_EmailValido", "[Email] LIKE '%@%' AND [Email] LIKE '%.%'");
            });

            builder.Property(u => u.Id).HasColumnType("INT");
            builder.Property(u => u.Nome).IsRequired().HasMaxLength(100).HasColumnType("VARCHAR(100)");
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100).HasColumnType("VARCHAR(100)");
            builder.Property(u => u.Senha).IsRequired().HasMaxLength(255).HasColumnType("VARCHAR(255)");
            builder.Property(u => u.Tipo).IsRequired().HasColumnName("TipoUsuario").HasColumnType("INT");
        }
    }
}
