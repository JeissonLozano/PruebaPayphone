using FluentValidation;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.CrearBilletera;

public class ValidadorCrearBilletera : AbstractValidator<CrearBilleteraComando>
{
    public ValidadorCrearBilletera()
    {
        RuleFor(x => x.DocumentoIdentidad)
            .NotEmpty().WithMessage("El documento de identidad es requerido.")
            .MaximumLength(20).WithMessage("El documento de identidad no puede tener más de 20 caracteres.")
            .Matches(@"^[A-Za-z0-9]+$").WithMessage("El documento de identidad solo puede contener letras y números.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.")
            .Matches(@"^[A-Za-zÀ-ÿ\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

        RuleFor(x => x.SaldoInicial)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo.")
            .PrecisionScale(18, 2, true).WithMessage("El saldo inicial debe tener máximo 2 decimales.");
    }
}