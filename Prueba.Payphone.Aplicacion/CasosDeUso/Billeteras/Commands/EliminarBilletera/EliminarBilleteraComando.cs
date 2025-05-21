using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Billeteras.Commands.EliminarBilletera;

public record EliminarBilleteraComando(int Id) : IRequest<ResultadoOperacion>;