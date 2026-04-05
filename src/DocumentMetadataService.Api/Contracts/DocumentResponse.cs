namespace DocumentMetadataService.Api.Contracts;

public sealed class DocumentResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty!;
    public string? Description { get; set; }
    public string FileName { get; set; } = string.Empty!;
    public string ContentType { get; set; } = string.Empty!;
    public long FileSizeBytes { get; set; }
    public string? ExternalReference { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
