using MediatR;
using Prueba.Payphone.Aplicacion.CasosDeUso.Comun;
using Prueba.Payphone.Dominio.Servicios.Autenticacion.Puertos;

namespace Prueba.Payphone.Aplicacion.CasosDeUso.Autenticacion.Commands.CerrarSesion;

public class CerrarSesionComandoHandler(
    IServicioAutenticacion servicioAutenticacion
) : IRequestHandler<CerrarSesionComando, RespuestaAutenticacionDto>
{
    public async Task<RespuestaAutenticacionDto> Handle(CerrarSesionComando comando, CancellationToken cancellationToken)
    {
        (bool exito, string mensaje) = await servicioAutenticacion.CerrarSesionAsync(comando.Token);

        return new RespuestaAutenticacionDto
        {
            Exito = exito,
            Mensaje = mensaje
        };
    }
}