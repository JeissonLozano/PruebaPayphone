using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Prueba.Payphone.Aplicacion;
using Prueba.Payphone.Aplicacion.Puertos;

namespace Prueba.Payphone.Infraestructura.Extensiones;


public static class ExtensionValidaciones
{
    public static IServiceCollection AgregarValidaciones(this IServiceCollection servicios)
    {
        servicios.AddValidatorsFromAssembly(typeof(ReferenciaEnsambladoAplicacion).Assembly);
        servicios.AddScoped(typeof(IPipelineBehavior<,>), typeof(ComportamientoValidacionMensaje<,>));

        return servicios;
    }
}

public sealed class ComportamientoValidacionMensaje<TMensaje, TRespuesta>(
    IEnumerable<IValidator<TMensaje>> validadores
) : IPipelineBehavior<TMensaje, TRespuesta>
    where TMensaje : IRequest<TRespuesta>, IRequiereValidacion
{
    private readonly IReadOnlyCollection<IValidator<TMensaje>> _validadores = validadores.ToList();

    public async Task<TRespuesta> Handle(
        TMensaje request,
        RequestHandlerDelegate<TRespuesta> next,
        CancellationToken cancellationToken)
    {
        ValidationContext<TMensaje> contexto = new ValidationContext<TMensaje>(request);
        List<ValidationFailure> resultadoValidacion = await _validadores.ValidarAsync(contexto, cancellationToken);

        if (resultadoValidacion.Count != 0)
        {
            throw new ValidationException(resultadoValidacion);
        }

        return await next(cancellationToken);
    }
}

public static class ExtensionesValidacion
{
    public static async Task<List<ValidationFailure>> ValidarAsync<TSolicitud>(
        this IEnumerable<IValidator<TSolicitud>> validadores,
        ValidationContext<TSolicitud> contextoValidacion,
        CancellationToken cancellationToken = default)
    {
        if (!validadores.Any())
        {
            return [];
        }

        ValidationResult[] resultadosValidacion = await Task.WhenAll(
            validadores.Select(validador =>
                validador.ValidateAsync(contextoValidacion, cancellationToken)));

        return [.. resultadosValidacion
            .SelectMany(resultado => resultado.Errors)
            .Where(error => error is not null)];
    }

    public static Dictionary<string, string[]> ADiccionario(this List<ValidationFailure>? errores)
    {
        return errores is not null && errores.Count != 0
            ? errores
                .GroupBy(
                    error => error.PropertyName,
                    error => error.ErrorMessage)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.ToArray())
            : [];
    }
}
