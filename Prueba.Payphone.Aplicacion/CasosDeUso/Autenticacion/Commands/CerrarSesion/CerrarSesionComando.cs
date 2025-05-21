using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.CerrarSesion;

public record CerrarSesionComando(string Token) : IRequest<RespuestaAutenticacionDto>;