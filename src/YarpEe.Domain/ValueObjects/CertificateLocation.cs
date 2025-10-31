namespace YarpEe.Domain.ValueObjects;

public record CertificateLocation
{
    public string Value { get; }

    public CertificateLocation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Certificate location cannot be empty", nameof(value));

        Value = value;
    }

    public static implicit operator string(CertificateLocation location) => location.Value;
    public static explicit operator CertificateLocation(string value) => new(value);

    public override string ToString() => Value;
}
