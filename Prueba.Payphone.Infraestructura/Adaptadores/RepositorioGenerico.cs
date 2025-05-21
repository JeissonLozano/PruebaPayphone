using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Infraestructura.Persistencia;
using Prueba.Payphone.Infraestructura.Puerto;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class RepositorioGenerico<T>(
    ContextoBaseDatos contexto
) : IRepositorioGenerico<T> where T : EntidadDominio
{
    private readonly char[] _separador = [','];

    public async Task<IEnumerable<T>> ObtenerTodosAsync(
        Expression<Func<T, bool>>? filtro = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? ordenarPor = null,
        string propiedadesIncluidas = "",
        bool conSeguimiento = false)
    {
        IQueryable<T> consulta = contexto.Set<T>();

        if (filtro != null)
        {
            consulta = consulta.Where(filtro);
        }

        if (!string.IsNullOrEmpty(propiedadesIncluidas))
        {
            foreach (string propiedad in propiedadesIncluidas.Split(_separador, StringSplitOptions.RemoveEmptyEntries))
            {
                consulta = consulta.Include(propiedad);
            }
        }

        if (ordenarPor != null)
        {
            return await ordenarPor(consulta).ToListAsync().ConfigureAwait(false);
        }

        return !conSeguimiento
            ? await consulta.AsNoTracking().ToListAsync()
            : await consulta.ToListAsync();
    }

    public async Task<T> ObtenerPorIdAsync(int id, string? propiedadesIncluidas = default)
    {
        IQueryable<T> consulta = contexto.Set<T>().AsQueryable();

        if (!string.IsNullOrEmpty(propiedadesIncluidas))
        {
            foreach (string propiedad in propiedadesIncluidas.Split(_separador, StringSplitOptions.RemoveEmptyEntries))
            {
                consulta = consulta.Include(propiedad);
            }
        }

        return await consulta.FirstOrDefaultAsync(e => e.Id == id) ?? default!;
    }

    public async Task<T> AgregarAsync(T entidad)
    {
        _ = entidad ?? throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
        await contexto.Set<T>().AddAsync(entidad);
        return entidad;
    }

    public async Task ActualizarAsync(T entidad)
    {
        _ = entidad ?? throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
        contexto.Set<T>().Update(entidad);
        await Task.CompletedTask;
    }

    public async Task EliminarAsync(T entidad)
    {
        _ = entidad ?? throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
        contexto.Set<T>().Remove(entidad);
        await Task.CompletedTask;
    }

    public Task<int> ObtenerCantidadAsync()
    {
        return contexto.Set<T>().CountAsync();
    }
}
