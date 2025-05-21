using FluentValidation;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Movimientos.Commands.CrearMovimiento;

public class CrearMovimientoComandoValidador : AbstractValidator<CrearMovimientoComando>
{
    public CrearMovimientoComandoValidador()
    {
        RuleFor(x => x.BilleteraId)
            .GreaterThan(0)
            .WithMessage("El ID de la billetera debe ser mayor a cero.");

        RuleFor(x => x.Monto)
            .NotEmpty()
            .WithMessage("El monto es requerido.")
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor a cero.")
            .PrecisionScale(18, 2, false)
            .WithMessage("El monto debe tener máximo 2 decimales.");

        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("El tipo de movimiento debe ser Débito o Crédito.");
    }
}