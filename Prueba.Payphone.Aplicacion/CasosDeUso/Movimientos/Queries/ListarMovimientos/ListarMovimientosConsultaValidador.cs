using FluentValidation;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Queries.ListarMovimientos;

public class ListarMovimientosConsultaValidador : AbstractValidator<ListarMovimientosConsulta>
{
    public ListarMovimientosConsultaValidador()
    {
        RuleFor(x => x.BilleteraId)
            .GreaterThan(0)
            .WithMessage("El ID de la billetera debe ser mayor a cero.");

        RuleFor(x => x.Pagina)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a cero.");

        RuleFor(x => x.ElementosPorPagina)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Los elementos por página deben estar entre 1 y 100.");

        RuleFor(x => x.FechaFin)
            .GreaterThanOrEqualTo(x => x.FechaInicio)
            .When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue)
            .WithMessage("La fecha fin debe ser mayor o igual a la fecha inicio.");
    }
}