namespace Prueba.Payphone.Aplicacion.Puertos;

public interface IUnidadDeTrabajo
{
    Task GuardarCambiosAsync(CancellationToken? cancellationToken = null);
}
