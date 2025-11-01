namespace YarpEe.UI.Models;

public class DestinationModel
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? HealthPath { get; set; }
}

public class DestinationRequest
{
    public string Address { get; set; } = string.Empty;
    public string? HealthPath { get; set; }
}
