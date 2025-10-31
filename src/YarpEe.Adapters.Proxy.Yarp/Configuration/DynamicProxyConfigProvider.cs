using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;
using YarpEe.Application.Ports;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Proxy.Yarp.Configuration;

public class DynamicProxyConfigProvider : IProxyConfigProvider
{
    private readonly IServiceProvider _serviceProvider;
    private volatile DynamicProxyConfig _config;
    private CancellationTokenSource _changeToken;

    public DynamicProxyConfigProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _changeToken = new CancellationTokenSource();
        _config = new DynamicProxyConfig(Array.Empty<RouteConfig>(), Array.Empty<ClusterConfig>());
    }

    public IProxyConfig GetConfig()
    {
        return _config;
    }

    public async Task ReloadConfigAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var routeRepository = scope.ServiceProvider.GetRequiredService<IRouteRepository>();
        var clusterRepository = scope.ServiceProvider.GetRequiredService<IClusterRepository>();
        var hostRepository = scope.ServiceProvider.GetRequiredService<IHostRepository>();
        
        var routes = await routeRepository.GetEnabledAsync();
        var clusters = await clusterRepository.GetAllAsync();
        var hosts = await hostRepository.GetAllAsync();

        var routeConfigs = new List<RouteConfig>();
        var clusterConfigs = new List<ClusterConfig>();

        foreach (var cluster in clusters)
        {
            var destinations = cluster.Destinations
                .Select(d => (d.Id.ToString(), new DestinationConfig
                {
                    Address = d.Address,
                    Health = d.HealthPath
                }))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var clusterConfig = new ClusterConfig
            {
                ClusterId = cluster.Id.ToString(),
                LoadBalancingPolicy = cluster.LoadBalancingPolicy,
                Destinations = destinations
            };

            clusterConfigs.Add(clusterConfig);
        }

        foreach (var route in routes)
        {
            var host = hosts.FirstOrDefault(h => h.Id == route.HostId);
            if (host == null) continue;

            var match = new RouteMatch
            {
                Path = route.Path.Value,
                Hosts = new[] { host.Name.Value }
            };

            if (route.Methods.Any())
            {
                match = match with { Methods = route.Methods.ToArray() };
            }

            var routeConfig = new RouteConfig
            {
                RouteId = route.Id.ToString(),
                ClusterId = route.ClusterId.ToString(),
                Match = match,
                Order = route.Order
            };

            routeConfigs.Add(routeConfig);
        }

        var oldConfig = _config;
        _config = new DynamicProxyConfig(routeConfigs, clusterConfigs);

        var oldChangeToken = _changeToken;
        _changeToken = new CancellationTokenSource();
        oldChangeToken.Cancel();
    }

    private class DynamicProxyConfig : IProxyConfig
    {
        private readonly CancellationChangeToken _changeToken;

        public DynamicProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            var cts = new CancellationTokenSource();
            _changeToken = new CancellationChangeToken(cts.Token);
        }

        public IReadOnlyList<RouteConfig> Routes { get; }
        public IReadOnlyList<ClusterConfig> Clusters { get; }
        public IChangeToken ChangeToken => _changeToken;
    }
}
