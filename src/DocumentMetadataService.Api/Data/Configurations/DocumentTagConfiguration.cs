using DocumentMetadataService.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentMetadataService.Api.Data.Configurations;

public class DocumentTagConfiguration : IEntityTypeConfiguration<DocumentTag>
{
    public void Configure(EntityTypeBuilder<DocumentTag> builder)
    {
        builder.ToTable("document_tags");

        builder.HasKey(x => new { x.DocumentId, x.TagId });

        builder.HasOne(x => x.Document)
            .WithMany(x => x.DocumentTags)
            .HasForeignKey(x => x.DocumentId);

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.DocumentTags)
            .HasForeignKey(x => x.TagId);

        builder.HasIndex(x => x.TagId);
    }
}
