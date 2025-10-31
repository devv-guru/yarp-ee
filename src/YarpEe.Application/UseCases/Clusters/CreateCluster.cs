using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;

namespace YarpEe.Application.UseCases.Clusters;

public class CreateCluster
{
    private readonly IClusterRepository _clusterRepository;
    private readonly IProxyConfigReloader _proxyConfigReloader;

    public CreateCluster(IClusterRepository clusterRepository, IProxyConfigReloader proxyConfigReloader)
    {
        _clusterRepository = clusterRepository;
        _proxyConfigReloader = proxyConfigReloader;
    }

    public async Task<Cluster> ExecuteAsync(Request request, CancellationToken cancellationToken = default)
    {
        var existingCluster = await _clusterRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingCluster != null)
            throw new InvalidOperationException($"Cluster with name '{request.Name}' already exists");

        var cluster = new Cluster(Guid.NewGuid(), request.Name, request.LoadBalancingPolicy ?? "RoundRobin");

        if (request.Destinations != null)
        {
            foreach (var dest in request.Destinations)
            {
                var destination = new Destination(Guid.NewGuid(), cluster.Id, dest.Address, dest.HealthPath);
                cluster.AddDestination(destination);
            }
        }

        await _clusterRepository.AddAsync(cluster, cancellationToken);
        await _proxyConfigReloader.ReloadAsync(cancellationToken);

        return cluster;
    }

    public record Request(string Name, string? LoadBalancingPolicy = null, IEnumerable<DestinationRequest>? Destinations = null);
    public record DestinationRequest(string Address, string? HealthPath = null);
}
