using YarpEe.Domain.ValueObjects;

namespace YarpEe.Domain.Entities;

public class Host
{
    public Guid Id { get; private set; }
    public HostName Name { get; private set; }
    public string BaseUrl { get; private set; }
    public string? CertificateRef { get; private set; }
    public DateTime CreatedUtc { get; private set; }
    public DateTime UpdatedUtc { get; private set; }

    private Host() { }

    public Host(Guid id, HostName name, string baseUrl, string? certificateRef = null)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        BaseUrl = baseUrl;
        CertificateRef = certificateRef;
        CreatedUtc = DateTime.UtcNow;
        UpdatedUtc = DateTime.UtcNow;
    }

    public void UpdateName(HostName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        UpdatedUtc = DateTime.UtcNow;
    }

    public void UpdateBaseUrl(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));

        BaseUrl = baseUrl;
        UpdatedUtc = DateTime.UtcNow;
    }

    public void UpdateCertificateRef(string? certificateRef)
    {
        CertificateRef = certificateRef;
        UpdatedUtc = DateTime.UtcNow;
    }
}
