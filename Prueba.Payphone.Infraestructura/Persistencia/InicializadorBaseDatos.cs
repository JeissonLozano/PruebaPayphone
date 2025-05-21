using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Prueba.Payphone.Infraestructura.Persistencia;

public class InicializadorBaseDatos(ILogger<InicializadorBaseDatos> logger, ContextoBaseDatos contexto)
{
    public async Task InicializadorBaseDatosAsync()
    {
        try
        {
            if (contexto.Database.IsRelational())
                await contexto.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await Task.Run(() => contexto.ChangeTracker.Clear());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}
