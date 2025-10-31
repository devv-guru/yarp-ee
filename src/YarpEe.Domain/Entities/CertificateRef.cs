using YarpEe.Domain.ValueObjects;

namespace YarpEe.Domain.Entities;

public class CertificateRef
{
    public Guid Id { get; private set; }
    public string Kind { get; private set; }
    public CertificateLocation Location { get; private set; }
    public string? PasswordSecret { get; private set; }

    private CertificateRef() { }

    public CertificateRef(Guid id, string kind, CertificateLocation location, string? passwordSecret = null)
    {
        if (string.IsNullOrWhiteSpace(kind))
            throw new ArgumentException("Certificate kind cannot be empty", nameof(kind));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Kind = kind;
        Location = location ?? throw new ArgumentNullException(nameof(location));
        PasswordSecret = passwordSecret;
    }

    public void UpdateLocation(CertificateLocation location)
    {
        Location = location ?? throw new ArgumentNullException(nameof(location));
    }

    public void UpdatePasswordSecret(string? passwordSecret)
    {
        PasswordSecret = passwordSecret;
    }
}
