namespace YarpEe.Application.Ports;

public interface IProxyConfigReloader
{
    Task ReloadAsync(CancellationToken cancellationToken = default);
}
