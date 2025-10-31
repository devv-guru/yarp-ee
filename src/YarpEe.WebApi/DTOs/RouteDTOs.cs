namespace YarpEe.WebApi.DTOs;

public record CreateRouteRequest(Guid HostId, Guid ClusterId, string Path, int Order = 0, bool Enabled = true, IEnumerable<string>? Methods = null);
public record UpdateRouteRequest(Guid? ClusterId = null, string? Path = null, int? Order = null, bool? Enabled = null, IEnumerable<string>? Methods = null);

public record RouteResponse(Guid Id, Guid HostId, Guid ClusterId, string Path, int Order, bool Enabled, IEnumerable<string> Methods);
