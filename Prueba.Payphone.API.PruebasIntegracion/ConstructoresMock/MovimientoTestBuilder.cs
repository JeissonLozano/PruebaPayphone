using Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.API.PruebasIntegracion.ConstructoresMock;

public static class MovimientoTestBuilder
{
    public static CrearMovimientoComando CrearComandoCredito(int billeteraId = 1, decimal monto = 100.00M) =>
        new(billeteraId, monto, TipoMovimiento.Credito);

    public static CrearMovimientoComando CrearComandoDebito(int billeteraId = 1, decimal monto = 50.00M) =>
        new(billeteraId, monto, TipoMovimiento.Debito);
} 