using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

namespace Prueba.Payphone.Infraestructura.Persistencia.Semillas;

public class SemillasBaseDatos(
    ILogger<SemillasBaseDatos> logger,
    ContextoBaseDatos context,
    IServicioPassword servicioPassword)
{
    public async Task InicializarAsync()
    {
        try
        {
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocurrió un error al inicializar la base de datos");
            throw;
        }
    }

    public async Task SembrarDatosAsync()
    {
        try
        {
            await SembrarUsuariosAsync();
            await SembrarBilleterasAsync();
            await SembrarMovimientosAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocurrió un error al sembrar los datos");
            throw;
        }
    }

    private async Task SembrarUsuariosAsync()
    {
        if (await context.Usuarios.AnyAsync())
            return;

        logger.LogInformation("Sembrando usuarios...");

        (string hashAdmin, string salAdmin) = servicioPassword.HashearPassword("Admin123!");
        (string hashUsuario1, string salUsuario1) = servicioPassword.HashearPassword("Usuario123!");
        (string hashUsuario2, string salUsuario2) = servicioPassword.HashearPassword("Usuario123!");

        List<Usuario> usuarios =
        [
            new Usuario
            {
                NombreUsuario = "admin",
                CorreoElectronico = "admin@payphone.com",
                HashClave = hashAdmin,
                Sal = salAdmin,
                Rol = "admin",
                FechaCreacion = DateTime.UtcNow,
                UltimoAcceso = null,
                Activo = true
            },
            new Usuario
            {
                NombreUsuario = "usuario1",
                CorreoElectronico = "juan.perez@payphone.com",
                HashClave = hashUsuario1,
                Sal = salUsuario1,
                Rol = "usuario",
                FechaCreacion = DateTime.UtcNow,
                UltimoAcceso = null,
                Activo = true
            },
            new Usuario
            {
                NombreUsuario = "usuario2",
                CorreoElectronico = "maria.garcia@payphone.com",
                HashClave = hashUsuario2,
                Sal = salUsuario2,
                Rol = "usuario",
                FechaCreacion = DateTime.UtcNow,
                UltimoAcceso = null,
                Activo = true
            }
        ];

        await context.Usuarios.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();
    }

    private async Task SembrarBilleterasAsync()
    {
        if (await context.Billeteras.AnyAsync())
            return;

        logger.LogInformation("Sembrando billeteras...");

        List<Billetera> billeteras =
        [
            new Billetera("1234567890", "Juan Pérez"),
            new Billetera("0987654321", "María García"),
            new Billetera("1122334455", "Carlos Rodríguez"),
            new Billetera("5544332211", "Ana Martínez"),
            new Billetera("9988776655", "Luis González")
        ];

        billeteras[0].Acreditar(1000.00m);
        billeteras[1].Acreditar(500.00m);
        billeteras[2].Acreditar(750.00m);
        billeteras[3].Acreditar(250.00m);
        billeteras[4].Acreditar(1500.00m);

        await context.Billeteras.AddRangeAsync(billeteras);
        await context.SaveChangesAsync();
    }

    private async Task SembrarMovimientosAsync()
    {
        if (await context.Movimientos.AnyAsync())
            return;

        logger.LogInformation("Sembrando movimientos...");

        List<Billetera> billeteras = await context.Billeteras.ToListAsync();
        List<Movimiento> movimientos = [];

        foreach (Billetera billetera in billeteras)
        {
            movimientos.Add(Movimiento.CrearCredito(billetera.Id, 100.00m));
            movimientos.Add(Movimiento.CrearDebito(billetera.Id, 50.00m));
            movimientos.Add(Movimiento.CrearCredito(billetera.Id, 75.00m));
        }

        await context.Movimientos.AddRangeAsync(movimientos);
        await context.SaveChangesAsync();
    }
}