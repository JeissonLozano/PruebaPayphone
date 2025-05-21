using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Infraestructura.Persistencia;

public class ContextoBaseDatos(
    DbContextOptions<ContextoBaseDatos> opciones,
    IConfiguration configuration
) : DbContext(opciones)
{
    public DbSet<Billetera> Billeteras => Set<Billetera>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            return;
        }

        modelBuilder.HasDefaultSchema(configuration.GetValue<string>("SchemaName"));

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>().HaveMaxLength(450);
    }
}