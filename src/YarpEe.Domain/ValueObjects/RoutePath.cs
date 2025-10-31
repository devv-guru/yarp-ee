namespace YarpEe.Domain.ValueObjects;

public record RoutePath
{
    public string Value { get; }

    public RoutePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Route path cannot be empty", nameof(value));

        if (!value.StartsWith('/'))
            throw new ArgumentException("Route path must start with /", nameof(value));

        Value = value;
    }

    public static implicit operator string(RoutePath path) => path.Value;
    public static explicit operator RoutePath(string value) => new(value);

    public override string ToString() => Value;
}
