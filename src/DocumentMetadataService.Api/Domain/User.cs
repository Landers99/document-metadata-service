namespace DocumentMetadataService.Api.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = ""!;
    public string PasswordHash { get; set; } = ""!;
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
