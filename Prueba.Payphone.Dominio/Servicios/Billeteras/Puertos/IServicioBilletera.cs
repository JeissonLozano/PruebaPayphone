using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;

public interface IServicioBilletera
{
    Task<(bool Exito, string Mensaje, Billetera? Billetera)> CrearBilleteraAsync(
        string documentoIdentidad,
        string nombre,
        decimal saldoInicial);

    Task<(bool Exito, string Mensaje, Billetera? Billetera)> ActualizarBilleteraAsync(
        int id,
        string documentoIdentidad,
        string nombre);

    Task<(bool Exito, string Mensaje)> EliminarBilleteraAsync(int id);
}