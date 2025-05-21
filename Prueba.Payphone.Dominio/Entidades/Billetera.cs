namespace Prueba.Payphone.Dominio.Entidades;

public class Billetera : EntidadDominio
{
    protected Billetera() { }

    public Billetera(string documentoIdentidad, string nombre)
    {
        if (string.IsNullOrWhiteSpace(documentoIdentidad))
            throw new ArgumentException($"'{nameof(documentoIdentidad)}' no puede estar vacío.", nameof(documentoIdentidad));

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException($"'{nameof(nombre)}' no puede estar vacío.", nameof(nombre));

        DocumentoIdentidad = documentoIdentidad;
        Nombre = nombre;
        Saldo = 0;
        FechaCreacion = DateTime.UtcNow;
        FechaActualizacion = DateTime.UtcNow;
    }

    public string DocumentoIdentidad { get; private set; } = null!;
    public string Nombre { get; private set; } = null!;
    public decimal Saldo { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaActualizacion { get; private set; }

    public void Debitar(decimal monto)
    {
        if (monto <= 0)
        {
            throw new ArgumentException($"'{nameof(monto)}' debe ser mayor que cero.", nameof(monto));
        }

        if (Saldo < monto)
        {
            throw new InvalidOperationException("Saldo insuficiente para realizar la operación.");
        }

        Saldo -= monto;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void Acreditar(decimal monto)
    {
        if (monto <= 0)
        {
            throw new ArgumentException($"'{nameof(monto)}' debe ser mayor que cero.", nameof(monto));
        }

        Saldo += monto;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void ActualizarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
        {
            throw new ArgumentException($"'{nameof(nuevoNombre)}' no puede estar vacío.", nameof(nuevoNombre));
        }

        Nombre = nuevoNombre;
        FechaActualizacion = DateTime.UtcNow;
    }
}