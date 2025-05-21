using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prueba.Payphone.API.Endpoints;
using Prueba.Payphone.Aplicacion;
using Prueba.Payphone.Infraestructura.Extensiones;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AgregarInfraestructura(builder.Configuration);

builder.Services.ConfigurarAutenticacion(builder.Configuration);
builder.Services.ConfigurarAutorizacion();
builder.Services.ConfigurarRateLimit();

builder.Services.AgregarValidaciones();

builder.ConfigurarOpenApi();

builder.Services.AddHealthChecks()
    .AddCheck("api", () => HealthCheckResult.Healthy("La API está funcionando correctamente"), tags: ["api"]);

builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
    options.Period = TimeSpan.FromSeconds(10);
});

builder.RegistrarSerilog();

builder.Services.AddValidatorsFromAssembly(typeof(ReferenciaEnsambladoAplicacion).Assembly);

builder.ConfigurarFiltroExepciones();

builder.ConfigurarCors();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ReferenciaEnsambladoAplicacion).Assembly));

builder.Services.AddAutoMapper(typeof(ReferenciaEnsambladoAplicacion));

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes.AssignableTo<IEndpointRegistrar>())
    .As<IEndpointRegistrar>()
    .WithScopedLifetime());

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "__Host-XSRF-TOKEN";
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

await app.InicializarBaseDeDatosAsync();

app.UseForwardedHeaders();

app.UseCors("PoliticaCors");

if (!app.Environment.IsDevelopment() && !app.Environment.EnvironmentName.Equals("Testing"))
{
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.ConfigurarMetricasSaludApi();

app.ConfiguracionEncabezadosSeguridad();

app.UseExceptionHandler();

app.MapEndpointDefinitions();

app.UseAntiforgery();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
