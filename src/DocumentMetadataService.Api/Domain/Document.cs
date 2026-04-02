namespace DocumentMetadataService.Api.Domain;

public class Document
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Title { get; set; } = ""!;
    public string? Description { get; set; }
    public string FileName { get; set; } = ""!;
    public string ContentType { get; set; } = ""!;
    public long FileSizeBytes { get; set; }
    public string? ExternalReference { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public User Owner { get; set; } = null!;
    public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
}
