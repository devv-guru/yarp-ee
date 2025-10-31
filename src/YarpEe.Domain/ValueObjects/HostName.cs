namespace YarpEe.Domain.ValueObjects;

public record HostName
{
    public string Value { get; }

    public HostName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Host name cannot be empty", nameof(value));

        Value = value;
    }

    public static implicit operator string(HostName hostName) => hostName.Value;
    public static explicit operator HostName(string value) => new(value);

    public override string ToString() => Value;
}
