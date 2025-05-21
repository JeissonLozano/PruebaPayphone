using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Prueba.Payphone.Infraestructura.ManejadorExepciones;

public class ManejadorProblemasExcepciones(
    IProblemDetailsService problemDetailsService
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        string instancia = $"{httpContext.Request.Method} {httpContext.Request.Path}";

        ProblemDetails? detalleError = exception switch
        {
            FluentValidation.ValidationException ex => new HttpValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación",
                Detail = "Uno o más errores de validación ocurrieron.",
                Instance = instancia,
                Errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            },

            UniqueConstraintException e => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Restricción única incumplida",
                Detail = e.ConstraintName != null && e.ConstraintProperties?.Any() == true
                ? $"Restricción única '{e.ConstraintName}' incumplida. Valor duplicado en el campo '{e.ConstraintProperties[0]}'."
                : e.ConstraintName != null
                ? $"Restricción única '{e.ConstraintName}' incumplida."
                : "Se ha incumplido una restricción única.",
                Instance = instancia
            },

            CannotInsertNullException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Campo obligatorio nulo",
                Detail = "Un campo obligatorio no fue proporcionado (valor nulo).",
                Instance = instancia
            },

            MaxLengthExceededException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Longitud máxima excedida",
                Detail = "Un valor excedió la longitud máxima permitida.",
                Instance = instancia
            },

            NumericOverflowException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Desbordamiento numérico",
                Detail = "Un valor numérico excedió el límite permitido.",
                Instance = instancia
            },

            ReferenceConstraintException e => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Violación de restricción de referencia",
                Detail = e.ConstraintName != null && e.ConstraintProperties?.Any() == true
                    ? $"Restricción de clave foránea '{e.ConstraintName}' violada. Columnas involucradas: {string.Join(", ", e.ConstraintProperties)}."
                    : e.ConstraintName != null
                        ? $"Restricción de clave foránea '{e.ConstraintName}' violada."
                        : "Se ha violado una restricción de referencia.",
                Instance = instancia
            },

            DbUpdateException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error al actualizar la base de datos",
                Detail = "Ocurrió un error al intentar guardar los cambios en la base de datos.",
                Instance = instancia
            },

            KeyNotFoundException ex => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso no encontrado",
                Detail = ex.Message,
                Instance = instancia
            },

            InvalidOperationException ex => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Operación inválida",
                Detail = ex.Message,
                Instance = instancia
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = "Ha ocurrido un error inesperado. Por favor, inténtelo más tarde.",
                Instance = instancia
            }
        };

        httpContext.Response.StatusCode = detalleError.Status ?? StatusCodes.Status400BadRequest;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = detalleError
        });
    }
}
