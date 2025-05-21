using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Infraestructura.Persistencia;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class UnidadDeTrabajo(ContextoBaseDatos contexto) : IUnidadDeTrabajo
{
    public async Task GuardarCambiosAsync(CancellationToken? cancellationToken = null)
    {
        CancellationToken token = cancellationToken ?? new CancellationTokenSource().Token;

        contexto.ChangeTracker.DetectChanges();

        Dictionary<EntityState, string> entryStatus = new()
        {
            {EntityState.Added, "FechaCreacion"},
            {EntityState.Modified, "FechaActualizacion"}
        };

        foreach (EntityEntry? entry in contexto.ChangeTracker.Entries().Where(entity => entryStatus.ContainsKey(entity.State)))
        {
            entry.Property(entryStatus[entry.State]).CurrentValue = DateTime.UtcNow;
        }

        await contexto.SaveChangesAsync(token);
    }
}
