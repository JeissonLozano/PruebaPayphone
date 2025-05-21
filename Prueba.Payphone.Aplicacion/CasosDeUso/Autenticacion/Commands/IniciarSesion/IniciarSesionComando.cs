using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.IniciarSesion;

public record IniciarSesionComando(
    string NombreUsuario,
    string Clave
) : IRequest<RespuestaAutenticacionDto>;