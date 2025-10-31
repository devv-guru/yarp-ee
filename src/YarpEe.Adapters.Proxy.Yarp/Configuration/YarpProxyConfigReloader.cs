using YarpEe.Application.Ports;

namespace YarpEe.Adapters.Proxy.Yarp.Configuration;

public class YarpProxyConfigReloader : IProxyConfigReloader
{
    private readonly DynamicProxyConfigProvider _configProvider;

    public YarpProxyConfigReloader(DynamicProxyConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        await _configProvider.ReloadConfigAsync();
    }
}
