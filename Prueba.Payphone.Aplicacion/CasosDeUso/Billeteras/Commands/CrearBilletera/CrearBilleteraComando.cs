using MediatR;
using Prueba.Payphone.Aplicacion.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.CrearBilletera;

public record CrearBilleteraComando(
    string DocumentoIdentidad,
    string Nombre,
    decimal SaldoInicial
) : IRequest<ResultadoOperacionBilletera>,
    IRequiereValidacion;