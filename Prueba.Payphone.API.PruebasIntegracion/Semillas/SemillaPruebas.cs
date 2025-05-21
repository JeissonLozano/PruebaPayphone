using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;
using Prueba.Payphone.Infraestructura.Persistencia;

namespace Prueba.Payphone.API.PruebasIntegracion.Semillas;

public class SemillaPruebas(
    ILogger<SemillaPruebas> logger,
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
            logger.LogError(ex, "Ocurrió un error al inicializar la base de datos de pruebas");
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
            logger.LogError(ex, "Ocurrió un error al sembrar los datos de prueba");
            throw;
        }
    }

    private async Task SembrarUsuariosAsync()
    {
        if (await context.Usuarios.AnyAsync())
            return;

        logger.LogInformation("Sembrando usuarios de prueba...");

        (string hashAdmin, string salAdmin) = servicioPassword.HashearPassword(DatosPrueba.Usuarios.AdminClave);
        (string hashUsuario, string salUsuario) = servicioPassword.HashearPassword(DatosPrueba.Usuarios.UsuarioClave);

        List<Usuario> usuarios =
        [
            new Usuario
            {
                NombreUsuario = DatosPrueba.Usuarios.AdminNombreUsuario,
                CorreoElectronico = "admin@test.com",
                HashClave = hashAdmin,
                Sal = salAdmin,
                Rol = "admin",
                FechaCreacion = DateTime.UtcNow,
                UltimoAcceso = null,
                Activo = true
            },
            new Usuario
            {
                NombreUsuario = DatosPrueba.Usuarios.UsuarioNombreUsuario,
                CorreoElectronico = "usuario@test.com",
                HashClave = hashUsuario,
                Sal = salUsuario,
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

        logger.LogInformation("Sembrando billeteras de prueba...");

        List<Billetera> billeteras =
        [
            new Billetera(DatosPrueba.Billeteras.DocumentoIdentidad1, DatosPrueba.Billeteras.Nombre1),
            new Billetera(DatosPrueba.Billeteras.DocumentoIdentidad2, DatosPrueba.Billeteras.Nombre2)
        ];

        billeteras[0].Acreditar(DatosPrueba.Billeteras.SaldoInicial1);
        billeteras[1].Acreditar(DatosPrueba.Billeteras.SaldoInicial2);

        await context.Billeteras.AddRangeAsync(billeteras);
        await context.SaveChangesAsync();
    }

    private async Task SembrarMovimientosAsync()
    {
        if (await context.Movimientos.AnyAsync())
            return;

        logger.LogInformation("Sembrando movimientos de prueba...");

        List<Billetera> billeteras = await context.Billeteras.ToListAsync();
        List<Movimiento> movimientos = [];

        // Crear movimientos para la primera billetera
        movimientos.Add(Movimiento.CrearCredito(billeteras[0].Id, DatosPrueba.Billeteras.SaldoInicial1));
        movimientos.Add(Movimiento.CrearDebito(billeteras[0].Id, 200.00M));

        // Crear movimientos para la segunda billetera
        movimientos.Add(Movimiento.CrearCredito(billeteras[1].Id, DatosPrueba.Billeteras.SaldoInicial2));

        await context.Movimientos.AddRangeAsync(movimientos);
        await context.SaveChangesAsync();
    }

    public static class DatosPrueba
    {
        public static class Usuarios
        {
            public const string AdminNombreUsuario = "admin";
            public const string AdminClave = "Admin123*";
            public const string UsuarioNombreUsuario = "usuario";
            public const string UsuarioClave = "Usuario123*";
        }

        public static class Billeteras
        {
            public const string DocumentoIdentidad1 = "1234567890";
            public const string Nombre1 = "Juan Pérez";
            public const string DocumentoIdentidad2 = "0987654321";
            public const string Nombre2 = "María López";
            public const decimal SaldoInicial1 = 1000.00M;
            public const decimal SaldoInicial2 = 500.00M;
        }
    }
} 