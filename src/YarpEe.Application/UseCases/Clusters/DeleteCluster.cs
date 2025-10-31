using YarpEe.Application.Ports;

namespace YarpEe.Application.UseCases.Clusters;

public class DeleteCluster
{
    private readonly IClusterRepository _clusterRepository;
    private readonly IProxyConfigReloader _proxyConfigReloader;

    public DeleteCluster(IClusterRepository clusterRepository, IProxyConfigReloader proxyConfigReloader)
    {
        _clusterRepository = clusterRepository;
        _proxyConfigReloader = proxyConfigReloader;
    }

    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cluster = await _clusterRepository.GetByIdAsync(id, cancellationToken);
        if (cluster == null)
            throw new InvalidOperationException($"Cluster with ID {id} not found");

        await _clusterRepository.DeleteAsync(id, cancellationToken);
        await _proxyConfigReloader.ReloadAsync(cancellationToken);
    }
}
