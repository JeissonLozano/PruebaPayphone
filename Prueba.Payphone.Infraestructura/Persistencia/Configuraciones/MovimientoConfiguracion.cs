using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Infraestructura.Persistencia.Configuraciones;

public class MovimientoConfiguracion : IEntityTypeConfiguration<Movimiento>
{
    public void Configure(EntityTypeBuilder<Movimiento> builder)
    {
        builder.ToTable("Movements");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BilleteraId)
            .HasColumnName("WalletId")
            .IsRequired()
            .HasComment("ID de la billetera asociada al movimiento");

        builder.Property(x => x.Monto)
            .HasColumnName("Amount")
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Monto del movimiento en la moneda base");

        builder.Property(x => x.Tipo)
            .HasColumnName("Type")
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasComment("Tipo de movimiento: Débito o Crédito");

        builder.Property(x => x.FechaCreacion)
            .HasColumnName("CreatedAt")
            .IsRequired()
            .HasComment("Fecha y hora de creación del movimiento");

        builder.HasIndex(x => x.BilleteraId)
            .HasDatabaseName("IX_Movements_WalletId");

        builder.HasOne<Billetera>()
            .WithMany()
            .HasForeignKey(x => x.BilleteraId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}