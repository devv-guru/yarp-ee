using YarpEe.UI.Models;

namespace YarpEe.UI.Services;

/// <summary>
/// Adapter implementation for Cluster operations using ApiClient
/// </summary>
public class ClusterService : IClusterService
{
    private readonly IApiClient _apiClient;
    private const string BaseEndpoint = "/api/clusters";

    public ClusterService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<ClusterModel>> GetAllClustersAsync()
    {
        var clusters = await _apiClient.GetAsync<List<ClusterModel>>(BaseEndpoint);
        return clusters ?? new List<ClusterModel>();
    }

    public async Task<ClusterModel?> GetClusterByIdAsync(Guid id)
    {
        return await _apiClient.GetAsync<ClusterModel>($"{BaseEndpoint}/{id}");
    }

    public async Task<ClusterModel?> CreateClusterAsync(CreateClusterRequest request)
    {
        return await _apiClient.PostAsync<ClusterModel>(BaseEndpoint, request);
    }

    public async Task<ClusterModel?> UpdateClusterAsync(Guid id, UpdateClusterRequest request)
    {
        return await _apiClient.PutAsync<ClusterModel>($"{BaseEndpoint}/{id}", request);
    }

    public async Task<bool> DeleteClusterAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"{BaseEndpoint}/{id}");
    }
}
