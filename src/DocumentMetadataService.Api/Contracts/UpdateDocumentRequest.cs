using System.ComponentModel.DataAnnotations;

namespace DocumentMetadataService.Api.Contracts;

public sealed class UpdateDocumentRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty!;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty!;

    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; } = string.Empty!;

    [Range(0, long.MaxValue)]
    public long FileSizeBytes { get; set; }

    [MaxLength(500)]
    public string? ExternalReference { get; set; }
}
