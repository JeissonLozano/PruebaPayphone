using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

public interface IBilleteraRepositorio
{
    Task<Billetera?> ObtenerPorIdAsync(int id);
    Task<Billetera?> ObtenerPorDocumentoIdentidadAsync(string documentoIdentidad);
    Task<List<Billetera>> ObtenerTodosAsync();
    Task<Billetera> AgregarAsync(Billetera billetera);
    Task ActualizarAsync(Billetera billetera);
    Task EliminarAsync(int id);
}