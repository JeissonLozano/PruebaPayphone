using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;
using Prueba.Payphone.Infraestructura.Puerto;

namespace Prueba.Payphone.Infraestructura.Adaptadores;

public class BilleteraRepositorio(
    IRepositorioGenerico<Billetera> repositorioGenerico
) : IBilleteraRepositorio
{
    public async Task<Billetera?> ObtenerPorIdAsync(int id)
    {
        return await repositorioGenerico.ObtenerPorIdAsync(id);
    }

    public async Task<Billetera?> ObtenerPorDocumentoIdentidadAsync(string documentoIdentidad)
    {
        IEnumerable<Billetera> billeteras = await repositorioGenerico.ObtenerTodosAsync(filtro: b => b.DocumentoIdentidad == documentoIdentidad);
        return billeteras.FirstOrDefault();
    }

    public async Task<List<Billetera>> ObtenerTodosAsync()
    {
        IEnumerable<Billetera> billeteras = await repositorioGenerico.ObtenerTodosAsync();
        return [.. billeteras];
    }

    public async Task<Billetera> AgregarAsync(Billetera billetera)
    {
        await repositorioGenerico.AgregarAsync(billetera);
        return billetera;
    }

    public async Task ActualizarAsync(Billetera billetera)
    {
        await repositorioGenerico.ActualizarAsync(billetera);
    }

    public async Task EliminarAsync(int id)
    {
        Billetera? billetera = await repositorioGenerico.ObtenerPorIdAsync(id);
        if (billetera != null)
        {
            await repositorioGenerico.EliminarAsync(billetera);
        }
    }
}