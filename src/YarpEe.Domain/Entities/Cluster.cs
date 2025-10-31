namespace YarpEe.Domain.Entities;

public class Cluster
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string LoadBalancingPolicy { get; private set; }
    public DateTime CreatedUtc { get; private set; }
    public DateTime UpdatedUtc { get; private set; }
    public ICollection<Destination> Destinations { get; private set; }

    private Cluster() 
    { 
        Destinations = new List<Destination>();
    }

    public Cluster(Guid id, string name, string loadBalancingPolicy = "RoundRobin")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Cluster name cannot be empty", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name;
        LoadBalancingPolicy = loadBalancingPolicy ?? "RoundRobin";
        CreatedUtc = DateTime.UtcNow;
        UpdatedUtc = DateTime.UtcNow;
        Destinations = new List<Destination>();
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Cluster name cannot be empty", nameof(name));

        Name = name;
        UpdatedUtc = DateTime.UtcNow;
    }

    public void UpdateLoadBalancingPolicy(string policy)
    {
        if (string.IsNullOrWhiteSpace(policy))
            throw new ArgumentException("Load balancing policy cannot be empty", nameof(policy));

        LoadBalancingPolicy = policy;
        UpdatedUtc = DateTime.UtcNow;
    }

    public void AddDestination(Destination destination)
    {
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));

        Destinations.Add(destination);
        UpdatedUtc = DateTime.UtcNow;
    }

    public void RemoveDestination(Guid destinationId)
    {
        var destination = Destinations.FirstOrDefault(d => d.Id == destinationId);
        if (destination != null)
        {
            Destinations.Remove(destination);
            UpdatedUtc = DateTime.UtcNow;
        }
    }
}
