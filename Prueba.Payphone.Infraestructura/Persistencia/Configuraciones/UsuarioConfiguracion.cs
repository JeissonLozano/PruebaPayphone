using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Infraestructura.Persistencia.Configuraciones;

public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NombreUsuario)
            .HasColumnName("Username")
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Nombre de usuario para autenticación");

        builder.Property(x => x.CorreoElectronico)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Dirección de correo electrónico del usuario");

        builder.Property(x => x.HashClave)
            .HasColumnName("PasswordHash")
            .IsRequired()
            .HasMaxLength(128)
            .HasComment("Contraseña hasheada");

        builder.Property(x => x.Sal)
            .HasColumnName("Salt")
            .IsRequired()
            .HasMaxLength(36)
            .HasComment("Sal para el hash de la contraseña");

        builder.Property(x => x.FechaCreacion)
            .HasColumnName("CreatedAt")
            .IsRequired()
            .HasComment("Fecha y hora de creación del usuario");

        builder.Property(x => x.UltimoAcceso)
            .HasColumnName("LastAccess")
            .HasComment("Fecha y hora del último acceso exitoso");

        builder.Property(x => x.Activo)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indica si la cuenta de usuario está activa");

        builder.HasIndex(x => x.NombreUsuario)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username");

        builder.HasIndex(x => x.CorreoElectronico)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");
    }
}