using AutoMapper;
using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.IniciarSesion;

public class IniciarSesionComandoHandler(
    IServicioAutenticacion servicioAutenticacion,
    IMapper mapper
) : IRequestHandler<IniciarSesionComando, RespuestaAutenticacionDto>
{
    public async Task<RespuestaAutenticacionDto> Handle(IniciarSesionComando comando, CancellationToken cancellationToken)
    {
        (bool exito, string mensaje, string token, Dominio.Entidades.Usuario usuario) = await servicioAutenticacion.IniciarSesionAsync(
            comando.NombreUsuario,
            comando.Clave);

        return new RespuestaAutenticacionDto
        {
            Exito = exito,
            Mensaje = mensaje,
            Token = token,
            Usuario = usuario != null ? mapper.Map<UsuarioDto>(usuario) : null
        };
    }
}