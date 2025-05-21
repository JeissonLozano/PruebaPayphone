using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.Dominio.Entidades;

public class Movimiento : EntidadDominio
{
    protected Movimiento() { }

    private Movimiento(int billeteraId, decimal monto, TipoMovimiento tipo)
    {
        if (billeteraId <= 0)
            throw new ArgumentException($"'{nameof(billeteraId)}' debe ser mayor que cero.", nameof(billeteraId));

        if (monto <= 0)
            throw new ArgumentException($"'{nameof(monto)}' debe ser mayor que cero.", nameof(monto));

        BilleteraId = billeteraId;
        Monto = monto;
        Tipo = tipo;
        FechaCreacion = DateTime.UtcNow;
    }

    public int BilleteraId { get; private set; }
    public decimal Monto { get; private set; }
    public TipoMovimiento Tipo { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    public static Movimiento CrearDebito(int billeteraId, decimal monto) =>
        new(billeteraId, monto, TipoMovimiento.Debito);

    public static Movimiento CrearCredito(int billeteraId, decimal monto) =>
        new(billeteraId, monto, TipoMovimiento.Credito);
}