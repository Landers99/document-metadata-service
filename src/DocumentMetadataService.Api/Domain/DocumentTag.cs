namespace DocumentMetadataService.Api.Domain;

public class DocumentTag
{
    public Guid DocumentId { get; set; }
    public int TagId { get; set; }

    public Document Document { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
