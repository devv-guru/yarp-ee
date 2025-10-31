using YarpEe.UI.Models;

namespace YarpEe.UI.Services;

/// <summary>
/// Adapter implementation for Route operations using ApiClient
/// </summary>
public class RouteService : IRouteService
{
    private readonly IApiClient _apiClient;
    private const string BaseEndpoint = "/api/routes";

    public RouteService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<RouteModel>> GetAllRoutesAsync()
    {
        var routes = await _apiClient.GetAsync<List<RouteModel>>(BaseEndpoint);
        return routes ?? new List<RouteModel>();
    }

    public async Task<RouteModel?> GetRouteByIdAsync(Guid id)
    {
        return await _apiClient.GetAsync<RouteModel>($"{BaseEndpoint}/{id}");
    }

    public async Task<RouteModel?> CreateRouteAsync(CreateRouteRequest request)
    {
        return await _apiClient.PostAsync<RouteModel>(BaseEndpoint, request);
    }

    public async Task<RouteModel?> UpdateRouteAsync(Guid id, UpdateRouteRequest request)
    {
        return await _apiClient.PutAsync<RouteModel>($"{BaseEndpoint}/{id}", request);
    }

    public async Task<bool> DeleteRouteAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"{BaseEndpoint}/{id}");
    }
}
