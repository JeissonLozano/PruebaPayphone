using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento;

public record CrearMovimientoComando(
    int BilleteraId,
    decimal Monto,
    TipoMovimiento Tipo
) : IRequest<MovimientoDto>,
    IRequiereValidacion;