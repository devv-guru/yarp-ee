using YarpEe.UI.Models;

namespace YarpEe.UI.Services;

/// <summary>
/// Port interface for Host management operations
/// </summary>
public interface IHostService
{
    Task<List<HostModel>> GetAllHostsAsync();
    Task<HostModel?> GetHostByIdAsync(Guid id);
}
