using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;

namespace YarpEe.Application.UseCases.Clusters;

public class UpdateCluster
{
    private readonly IClusterRepository _clusterRepository;
    private readonly IProxyConfigReloader _proxyConfigReloader;

    public UpdateCluster(IClusterRepository clusterRepository, IProxyConfigReloader proxyConfigReloader)
    {
        _clusterRepository = clusterRepository;
        _proxyConfigReloader = proxyConfigReloader;
    }

    public async Task<Cluster> ExecuteAsync(Request request, CancellationToken cancellationToken = default)
    {
        var cluster = await _clusterRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cluster == null)
            throw new InvalidOperationException($"Cluster with ID {request.Id} not found");

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != cluster.Name)
        {
            var existingCluster = await _clusterRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingCluster != null && existingCluster.Id != cluster.Id)
                throw new InvalidOperationException($"Cluster with name '{request.Name}' already exists");

            cluster.UpdateName(request.Name);
        }

        if (!string.IsNullOrWhiteSpace(request.LoadBalancingPolicy))
        {
            cluster.UpdateLoadBalancingPolicy(request.LoadBalancingPolicy);
        }

        await _clusterRepository.UpdateAsync(cluster, cancellationToken);
        await _proxyConfigReloader.ReloadAsync(cancellationToken);

        return cluster;
    }

    public record Request(Guid Id, string? Name = null, string? LoadBalancingPolicy = null);
}
