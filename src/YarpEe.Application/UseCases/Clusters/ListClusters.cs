using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;

namespace YarpEe.Application.UseCases.Clusters;

public class ListClusters
{
    private readonly IClusterRepository _clusterRepository;

    public ListClusters(IClusterRepository clusterRepository)
    {
        _clusterRepository = clusterRepository;
    }

    public async Task<IEnumerable<Cluster>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _clusterRepository.GetAllAsync(cancellationToken);
    }
}
