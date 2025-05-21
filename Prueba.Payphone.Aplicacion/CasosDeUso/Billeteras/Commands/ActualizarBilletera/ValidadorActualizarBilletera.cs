using FluentValidation;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.ActualizarBilletera;

public class ValidadorActualizarBilletera : AbstractValidator<ActualizarBilleteraComando>
{
    public ValidadorActualizarBilletera()
    {
        RuleFor(x => x.DocumentoIdentidad)
            .NotEmpty().WithMessage("El documento de identidad es requerido.")
            .MaximumLength(20).WithMessage("El documento de identidad no puede tener más de 20 caracteres.")
            .Matches(@"^[A-Za-z0-9]+$").WithMessage("El documento de identidad solo puede contener letras y números.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.")
            .Matches(@"^[A-Za-zÀ-ÿ\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");
    }
}