namespace YarpEe.UI.Models;

public class RouteModel
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public Guid ClusterId { get; set; }
    public string Path { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool Enabled { get; set; } = true;
    public List<string> Methods { get; set; } = new();
    
    // For display purposes
    public string? ClusterName { get; set; }
    public string? HostName { get; set; }
}

public class CreateRouteRequest
{
    public Guid HostId { get; set; }
    public Guid ClusterId { get; set; }
    public string Path { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool Enabled { get; set; } = true;
    public List<string>? Methods { get; set; }
}

public class UpdateRouteRequest
{
    public Guid? ClusterId { get; set; }
    public string? Path { get; set; }
    public int? Order { get; set; }
    public bool? Enabled { get; set; }
    public List<string>? Methods { get; set; }
}
