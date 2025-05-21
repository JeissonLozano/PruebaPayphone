using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

namespace Prueba.Payphone.Dominio.Servicios.Billeteras;

public sealed class ServicioBilletera(IBilleteraRepositorio repositorioBilletera) : IServicioBilletera
{
    public async Task<(bool Exito, string Mensaje, Billetera? Billetera)> CrearBilleteraAsync(
        string documentoIdentidad,
        string nombre,
        decimal saldoInicial)
    {
        Billetera? billeteraExistente = await repositorioBilletera.ObtenerPorDocumentoIdentidadAsync(documentoIdentidad);
        if (billeteraExistente is not null)
        {
            return (false, "Ya existe una billetera con ese documento.", null);
        }

        Billetera nuevaBilletera = new(documentoIdentidad, nombre);

        if (saldoInicial > 0)
        {
            nuevaBilletera.Acreditar(saldoInicial);
        }

        Billetera billeteraCreada = await repositorioBilletera.AgregarAsync(nuevaBilletera);
        return (true, "Billetera creada exitosamente.", billeteraCreada);
    }

    public async Task<List<Billetera>> ObtenerTodasAsync()
    {
        List<Billetera> billeteras = await repositorioBilletera.ObtenerTodosAsync();
        return billeteras;
    }

    public async Task<Billetera?> ObtenerPorIdAsync(int id)
    {
        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(id);
        return billetera;
    }

    public async Task<(bool Exito, string Mensaje, Billetera? Billetera)> ActualizarBilleteraAsync(
        int id,
        string documentoIdentidad,
        string nombre)
    {
        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(id);
        if (billetera is null)
        {
            return (false, "La billetera no existe.", null);
        }

        Billetera? billeteraExistente = await repositorioBilletera.ObtenerPorDocumentoIdentidadAsync(documentoIdentidad);
        if (billeteraExistente is not null && billeteraExistente.Id != id)
        {
            return (false, "Ya existe una billetera con ese documento de identidad.", null);
        }

        billetera.ActualizarNombre(nombre);

        await repositorioBilletera.ActualizarAsync(billetera);
        return (true, "Billetera actualizada exitosamente.", billetera);
    }

    public async Task<(bool Exito, string Mensaje)> EliminarBilleteraAsync(int id)
    {
        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(id);
        if (billetera is null)
        {
            return (false, "La billetera no existe.");
        }

        await repositorioBilletera.EliminarAsync(id);
        return (true, "Billetera eliminada exitosamente.");
    }
}