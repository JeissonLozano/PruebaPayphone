using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Servicios.Autenticacion;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;
using Prueba.Payphone.Dominio.Servicios.Billeteras;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;
using Prueba.Payphone.Dominio.Servicios.Movimientos;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;
using Prueba.Payphone.Infraestructura.Adaptadores;
using Prueba.Payphone.Infraestructura.Configuracion;
using Prueba.Payphone.Infraestructura.Persistencia;
using Prueba.Payphone.Infraestructura.Persistencia.Semillas;
using Prueba.Payphone.Infraestructura.Puerto;

namespace Prueba.Payphone.Infraestructura.Extensiones;

public static class ExtensionesInyeccionDependencias
{
    public static IServiceCollection AgregarInfraestructura(
        this IServiceCollection servicios,
        IConfiguration configuracion
    )
    {
        servicios
            .AgregarBaseDatos(configuracion)
            .AgregarRepositorios(configuracion)
            .AgregarServicios(configuracion)
            .AgregarOpcionesConfiguracion(configuracion)
            .AgregarConfiguraciones(configuracion);

        return servicios;
    }

    public static IServiceCollection AgregarRepositorios(
        this IServiceCollection servicios,
        IConfiguration configuracion
    )
    {
        servicios.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
        servicios.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();
        servicios.AddScoped<IBilleteraRepositorio, BilleteraRepositorio>();
        servicios.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

        servicios.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
        servicios.AddScoped<SemillasBaseDatos>();

        return servicios;
    }

    public static IServiceCollection AgregarServicios(
        this IServiceCollection servicios,
        IConfiguration configuracion
    )
    {
        servicios.AddScoped<IServicioBilletera, ServicioBilletera>();
        servicios.AddScoped<IServicioMovimiento, ServicioMovimiento>();
        servicios.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();

        servicios.AddSingleton<IServicioToken, ServicioTokenAdapter>();
        servicios.AddSingleton<IServicioPassword, ServicioPasswordAdapter>();

        return servicios;
    }

    private static IServiceCollection AgregarBaseDatos(
        this IServiceCollection servicios,
        IConfiguration configuracion
    )
    {
        servicios.AddDbContext<ContextoBaseDatos>((proveedor, opciones) =>
        {
            ExceptionProcessorExtensions.UseExceptionProcessor(new DbContextOptionsBuilder<ContextoBaseDatos>());

            opciones.UseSqlServer(configuracion.GetConnectionString("database"), sqlopts =>
            {
                sqlopts.MigrationsHistoryTable("_MigrationHistory", configuracion.GetValue<string>("SchemaName"));
            });

            opciones.AddInterceptors(proveedor.GetServices<ISaveChangesInterceptor>());
            opciones.EnableSensitiveDataLogging();
            opciones.EnableDetailedErrors();
        });

        servicios.AddScoped<InicializadorBaseDatos>();

        return servicios;
    }

    private static IServiceCollection AgregarConfiguraciones(
        this IServiceCollection servicios,
        IConfiguration configuracion
    )
    {
        servicios.Configure<JwtConfiguracion>(configuracion.GetSection("JWT"));
        return servicios;
    }


    public static async Task InicializarBaseDeDatosAsync(this WebApplication webApplication)
    {
        using IServiceScope ambito = webApplication.Services.CreateScope();
        SemillasBaseDatos inicializador = ambito.ServiceProvider.GetRequiredService<SemillasBaseDatos>();
        await inicializador.InicializarAsync().ConfigureAwait(false);

        IHostEnvironment entorno = webApplication.Services.GetRequiredService<IHostEnvironment>();
        if (entorno.IsDevelopment())
        {
            await inicializador.SembrarDatosAsync().ConfigureAwait(false);
        }
    }
}

