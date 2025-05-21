using System.Linq.Expressions;

namespace Prueba.Payphone.Infraestructura.Puerto;

public interface IRepositorioGenerico<T>
{
    Task<T> ObtenerPorIdAsync(
        int id,
        string? propiedadesIncluidas = default
    );

    Task<IEnumerable<T>> ObtenerTodosAsync(
        Expression<Func<T, bool>>? filtro = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? ordenarPor = null,
        string propiedadesIncluidas = "",
        bool conSeguimiento = false
    );

    Task<T> AgregarAsync(T entidad);

    Task ActualizarAsync(T entidad);

    Task EliminarAsync(T entidad);

    Task<int> ObtenerCantidadAsync();
}
