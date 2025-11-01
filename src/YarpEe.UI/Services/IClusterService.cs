using YarpEe.UI.Models;

namespace YarpEe.UI.Services;

/// <summary>
/// Port interface for Cluster management operations
/// </summary>
public interface IClusterService
{
    Task<List<ClusterModel>> GetAllClustersAsync();
    Task<ClusterModel?> GetClusterByIdAsync(Guid id);
    Task<ClusterModel?> CreateClusterAsync(CreateClusterRequest request);
    Task<ClusterModel?> UpdateClusterAsync(Guid id, UpdateClusterRequest request);
    Task<bool> DeleteClusterAsync(Guid id);
}
