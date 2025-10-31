namespace YarpEe.UI.Models;

public class ClusterModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LoadBalancingPolicy { get; set; } = "RoundRobin";
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public List<DestinationModel> Destinations { get; set; } = new();
}

public class CreateClusterRequest
{
    public string Name { get; set; } = string.Empty;
    public string? LoadBalancingPolicy { get; set; }
    public List<DestinationRequest>? Destinations { get; set; }
}

public class UpdateClusterRequest
{
    public string? Name { get; set; }
    public string? LoadBalancingPolicy { get; set; }
}
