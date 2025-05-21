using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Infraestructura.Persistencia.Configuraciones;

public class BilleteraConfiguracion : IEntityTypeConfiguration<Billetera>
{
    public void Configure(EntityTypeBuilder<Billetera> builder)
    {
        builder.ToTable("Wallets");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DocumentoIdentidad)
            .HasColumnName("DocumentId")
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Documento de identidad del propietario de la billetera");

        builder.Property(x => x.Nombre)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombre completo del propietario de la billetera");

        builder.Property(x => x.Saldo)
            .HasColumnName("Balance")
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Saldo actual de la billetera en moneda base");

        builder.Property(x => x.FechaCreacion)
            .HasColumnName("CreatedAt")
            .IsRequired()
            .HasComment("Fecha y hora de creación de la billetera");

        builder.Property(x => x.FechaActualizacion)
            .HasColumnName("UpdatedAt")
            .IsRequired()
            .HasComment("Fecha y hora de última actualización de la billetera");

        builder.HasIndex(x => x.DocumentoIdentidad)
            .IsUnique()
            .HasDatabaseName("IX_Wallets_DocumentId");
    }
}