namespace YarpEe.WebApi.DTOs;

public record CreateClusterRequest(string Name, string? LoadBalancingPolicy = null, IEnumerable<DestinationRequest>? Destinations = null);
public record UpdateClusterRequest(string? Name = null, string? LoadBalancingPolicy = null);
public record DestinationRequest(string Address, string? HealthPath = null);

public record ClusterResponse(Guid Id, string Name, string LoadBalancingPolicy, DateTime CreatedUtc, DateTime UpdatedUtc, IEnumerable<DestinationResponse> Destinations);
public record DestinationResponse(Guid Id, string Address, string? HealthPath);
