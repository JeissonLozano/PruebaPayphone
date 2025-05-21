using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prueba.Payphone.Infraestructura.ConfiguracionOpciones;

namespace Prueba.Payphone.Infraestructura.Extensiones;

public static class ExtensionConfiguracionOpciones
{
    public static IServiceCollection AgregarOpcionesConfiguracion(this IServiceCollection servicios, IConfiguration configuracion)
    {
        Dictionary<Type, string> mapeoOpciones = new()
        {
            { typeof(CorsOpciones), CorsOpciones.SECCION },
            { typeof(OpenApiOpciones), OpenApiOpciones.SECCION }
        };

        foreach (KeyValuePair<Type, string> par in mapeoOpciones)
        {
            Type tipo = par.Key;
            string seccion = par.Value;

            MethodInfo? metodoConfigure = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", [typeof(IServiceCollection), typeof(IConfigurationSection)])
                ?.MakeGenericMethod(tipo);

            metodoConfigure?.Invoke(null, [servicios, configuracion.GetSection(seccion)]);
        }

        return servicios;
    }
}
