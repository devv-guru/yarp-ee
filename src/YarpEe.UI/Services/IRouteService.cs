using YarpEe.UI.Models;

namespace YarpEe.UI.Services;

/// <summary>
/// Port interface for Route management operations
/// </summary>
public interface IRouteService
{
    Task<List<RouteModel>> GetAllRoutesAsync();
    Task<RouteModel?> GetRouteByIdAsync(Guid id);
    Task<RouteModel?> CreateRouteAsync(CreateRouteRequest request);
    Task<RouteModel?> UpdateRouteAsync(Guid id, UpdateRouteRequest request);
    Task<bool> DeleteRouteAsync(Guid id);
}
