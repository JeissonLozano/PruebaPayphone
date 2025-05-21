using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prueba.Payphone.API.PruebasIntegracion.Semillas;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;
using Prueba.Payphone.Infraestructura.Persistencia;

namespace Prueba.Payphone.API.PruebasIntegracion;

public class BasePruebasIntegracion : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ES_PRUEBA_INTEGRACION", "true");

        builder.ConfigureServices(ConfigurarServicios);
        builder.ConfigureAppConfiguration(ConfigurarConfiguracion);

        return base.CreateHost(builder);
    }

    private static void ConfigurarServicios(IServiceCollection services)
    {
        EliminarRegistrosDbContext(services);
        ConfigurarBaseDatosEnMemoria(services);
        InicializarBaseDatos(services);
    }

    private static void EliminarRegistrosDbContext(IServiceCollection services)
    {
        List<ServiceDescriptor> descriptores = [.. services.Where(d =>
            d.ServiceType == typeof(DbContextOptions<ContextoBaseDatos>) ||
            d.ServiceType == typeof(DbContextOptions) ||
            d.ServiceType == typeof(ContextoBaseDatos))];

        foreach (ServiceDescriptor descriptor in descriptores)
        {
            services.Remove(descriptor);
        }
    }

    private static void ConfigurarBaseDatosEnMemoria(IServiceCollection services)
    {
        ServiceProvider proveedorServicio = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        services.AddDbContext<ContextoBaseDatos>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting")
                   .UseInternalServiceProvider(proveedorServicio);
        });
    }

    private static void InicializarBaseDatos(IServiceCollection services)
    {
        using ServiceProvider sp = services.BuildServiceProvider();
        using IServiceScope scope = sp.CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        ContextoBaseDatos db = scopedServices.GetRequiredService<ContextoBaseDatos>();
        ILogger<SemillaPruebas> logger = scopedServices.GetRequiredService<ILogger<SemillaPruebas>>();
        IServicioPassword servicioPassword = scopedServices.GetRequiredService<IServicioPassword>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        SemillaPruebas semillaPruebas = new(logger, db, servicioPassword);
        semillaPruebas.InicializarAsync().GetAwaiter().GetResult();
        semillaPruebas.SembrarDatosAsync().GetAwaiter().GetResult();
    }

    private static void ConfigurarConfiguracion(HostBuilderContext context, IConfigurationBuilder config)
    {
        List<KeyValuePair<string, string?>> configuracionPruebas =
        [
            new("KeyVault:Nombre", "kv-mock-test"),
            new("ConnectionStrings:SqlDatabase", "Server=localhost;Database=TestDb;Trusted_Connection=True;"),
            new("JwtSettings:SecretKey", "clave-secreta-pruebas-integracion-12345"),
            new("JwtSettings:Issuer", "pruebas-integracion"),
            new("JwtSettings:Audience", "pruebas-integracion"),
            new("JwtSettings:ExpirationMinutes", "60")
        ];

        config.AddInMemoryCollection(configuracionPruebas);
    }

    protected HttpClient CrearCliente() => CreateClient();

    protected async Task<string> ObtenerTokenAutenticacionAsync(string nombreUsuario, string clave)
    {
        HttpClient cliente = CrearCliente();
        StringContent contenido = new(
            JsonSerializer.Serialize(new { nombreUsuario, clave }),
            Encoding.UTF8,
            "application/json");

        HttpResponseMessage respuesta = await cliente.PostAsync("/api/auth/login", contenido);
        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();
        
        if (!respuesta.IsSuccessStatusCode)
            throw new InvalidOperationException($"Error al obtener token: {contenidoRespuesta}");

        RespuestaAutenticacionDto? resultado = JsonSerializer.Deserialize<RespuestaAutenticacionDto>(
            contenidoRespuesta,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return resultado?.Token ?? throw new InvalidOperationException("Token no encontrado en la respuesta");
    }
}

public record RespuestaAutenticacionDto(bool Exito, string Mensaje, string? Token);