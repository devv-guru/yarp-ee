using YarpEe.UI.Models;

namespace YarpEe.UI.Services;

/// <summary>
/// Adapter implementation for Host operations using ApiClient
/// </summary>
public class HostService : IHostService
{
    private readonly IApiClient _apiClient;
    private const string BaseEndpoint = "/api/hosts";

    public HostService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<HostModel>> GetAllHostsAsync()
    {
        var hosts = await _apiClient.GetAsync<List<HostModel>>(BaseEndpoint);
        return hosts ?? new List<HostModel>();
    }

    public async Task<HostModel?> GetHostByIdAsync(Guid id)
    {
        return await _apiClient.GetAsync<HostModel>($"{BaseEndpoint}/{id}");
    }
}
