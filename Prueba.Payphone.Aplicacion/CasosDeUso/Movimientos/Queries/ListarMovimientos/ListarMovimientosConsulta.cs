using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Aplicacion.Puertos;
using Prueba.Payphone.Dominio.Enumeradores;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;

public record ListarMovimientosConsulta(
    int BilleteraId,
    int Pagina = 1,
    int ElementosPorPagina = 10,
    DateTime? FechaInicio = null,
    DateTime? FechaFin = null,
    TipoMovimiento? TipoMovimiento = null
) : IRequest<ResultadoPaginado<MovimientoDto>>,
    IRequiereValidacion;