using Prueba.Payphone.Dominio.Entidades;

namespace Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

public interface IServicioMovimiento
{
    Task<(bool Exito, string Mensaje, Movimiento? Movimiento)> CrearDebitoAsync(
        int billeteraId,
        decimal monto);

    Task<(bool Exito, string Mensaje, Movimiento? Movimiento)> CrearCreditoAsync(
        int billeteraId,
        decimal monto);

    Task<List<Movimiento>> ObtenerPorBilleteraAsync(int billeteraId);
}