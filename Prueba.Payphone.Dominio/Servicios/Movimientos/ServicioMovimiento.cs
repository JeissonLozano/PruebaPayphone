using Prueba.Payphone.Dominio.Entidades;
using Prueba.Payphone.Dominio.Servicios.Billeteras.Puertos;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

namespace Prueba.Payphone.Dominio.Servicios.Movimientos;

public sealed class ServicioMovimiento(
    IMovimientoRepositorio repositorioMovimiento,
    IBilleteraRepositorio repositorioBilletera) : IServicioMovimiento
{
    public async Task<(bool Exito, string Mensaje, Movimiento? Movimiento)> CrearDebitoAsync(
        int billeteraId,
        decimal monto)
    {
        if (monto <= 0)
            throw new ArgumentException($"'{nameof(monto)}' debe ser mayor que cero.", nameof(monto));

        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(billeteraId);

        if (billetera is null)
        {
            return (false, "La billetera no existe.", null);
        }

        if (billetera.Saldo < monto)
        {
            return (false, "Saldo insuficiente para realizar la operación.", null);
        }

        billetera.Debitar(monto);
        Movimiento movimientoDebito = Movimiento.CrearDebito(billeteraId, monto);

        await repositorioMovimiento.AgregarAsync(movimientoDebito);
        await repositorioBilletera.ActualizarAsync(billetera);

        return (true, "Débito realizado exitosamente.", movimientoDebito);
    }

    public async Task<(bool Exito, string Mensaje, Movimiento? Movimiento)> CrearCreditoAsync(
        int billeteraId,
        decimal monto)
    {
        if (monto <= 0)
            throw new ArgumentException($"'{nameof(monto)}' debe ser mayor que cero.", nameof(monto));

        Billetera? billetera = await repositorioBilletera.ObtenerPorIdAsync(billeteraId);

        if (billetera is null)
        {
            return (false, "La billetera no existe.", null);
        }

        billetera.Acreditar(monto);
        Movimiento movimientoCredito = Movimiento.CrearCredito(billeteraId, monto);

        await repositorioMovimiento.AgregarAsync(movimientoCredito);
        await repositorioBilletera.ActualizarAsync(billetera);

        return (true, "Crédito realizado exitosamente.", movimientoCredito);
    }

    public async Task<List<Movimiento>> ObtenerPorBilleteraAsync(int billeteraId)
    {
        List<Movimiento> movimientos = await repositorioMovimiento.ObtenerPorBilleteraAsync(billeteraId);
        return movimientos;
    }
}