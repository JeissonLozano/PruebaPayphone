using MediatR;
using Prueba.Payphone.Aplicacion.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.ActualizarBilletera;

public record ActualizarBilleteraComando(
    int Id,
    string Nombre,
    string DocumentoIdentidad
) : IRequest<ResultadoOperacionBilletera>,
    IRequiereValidacion;