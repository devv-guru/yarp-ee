namespace YarpEe.Domain.Entities;

public class Destination
{
    public Guid Id { get; private set; }
    public Guid ClusterId { get; private set; }
    public string Address { get; private set; }
    public string? HealthPath { get; private set; }

    private Destination() { }

    public Destination(Guid id, Guid clusterId, string address, string? healthPath = null)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Destination address cannot be empty", nameof(address));

        if (!Uri.TryCreate(address, UriKind.Absolute, out _))
            throw new ArgumentException("Destination address must be a valid URI", nameof(address));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        ClusterId = clusterId;
        Address = address;
        HealthPath = healthPath;
    }

    public void UpdateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Destination address cannot be empty", nameof(address));

        if (!Uri.TryCreate(address, UriKind.Absolute, out _))
            throw new ArgumentException("Destination address must be a valid URI", nameof(address));

        Address = address;
    }

    public void UpdateHealthPath(string? healthPath)
    {
        HealthPath = healthPath;
    }
}
