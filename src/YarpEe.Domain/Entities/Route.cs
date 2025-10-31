using YarpEe.Domain.ValueObjects;

namespace YarpEe.Domain.Entities;

public class Route
{
    public Guid Id { get; private set; }
    public Guid HostId { get; private set; }
    public Guid ClusterId { get; private set; }
    public RoutePath Path { get; private set; }
    public int Order { get; private set; }
    public ICollection<string> Methods { get; private set; }
    public bool Enabled { get; private set; }

    private Route() 
    { 
        Methods = new List<string>();
    }

    public Route(Guid id, Guid hostId, Guid clusterId, RoutePath path, int order = 0, bool enabled = true)
    {
        if (hostId == Guid.Empty)
            throw new ArgumentException("Host ID cannot be empty", nameof(hostId));

        if (clusterId == Guid.Empty)
            throw new ArgumentException("Cluster ID cannot be empty", nameof(clusterId));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        HostId = hostId;
        ClusterId = clusterId;
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Order = order;
        Methods = new List<string>();
        Enabled = enabled;
    }

    public void UpdatePath(RoutePath path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public void UpdateOrder(int order)
    {
        Order = order;
    }

    public void UpdateCluster(Guid clusterId)
    {
        if (clusterId == Guid.Empty)
            throw new ArgumentException("Cluster ID cannot be empty", nameof(clusterId));

        ClusterId = clusterId;
    }

    public void SetMethods(IEnumerable<string> methods)
    {
        Methods = methods?.ToList() ?? new List<string>();
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }
}
