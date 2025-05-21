using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Enumeradores;
using Prueba.Payphone.Dominio.Servicios.Movimientos.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento;

public class ManejadorCrearMovimiento(
    IServicioMovimiento servicioMovimiento,
    IUnidadDeTrabajo unidadDeTrabajo
) : IRequestHandler<CrearMovimientoComando, MovimientoDto>
{
    public async Task<MovimientoDto> Handle(CrearMovimientoComando comando, CancellationToken cancellationToken)
    {
        (bool exito, string mensaje, Dominio.Entidades.Movimiento movimiento) = comando.Tipo switch
        {
            TipoMovimiento.Credito => await servicioMovimiento.CrearCreditoAsync(comando.BilleteraId, comando.Monto),
            TipoMovimiento.Debito => await servicioMovimiento.CrearDebitoAsync(comando.BilleteraId, comando.Monto),
            _ => throw new InvalidOperationException($"Tipo de movimiento no soportado: {comando.Tipo}")
        };

        if (!exito)
        {
            throw new InvalidOperationException(mensaje);
        }

        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return new MovimientoDto
        {
            Id = movimiento!.Id,
            BilleteraId = movimiento.BilleteraId,
            Monto = movimiento.Monto,
            Tipo = movimiento.Tipo,
            FechaCreacion = movimiento.FechaCreacion
        };
    }
}