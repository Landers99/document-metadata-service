using DocumentMetadataService.Api.Contracts;
using DocumentMetadataService.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentMetadataService.Api.Data;

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _dbContext;

    public DocumentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DocumentResponse> CreateAsync(
            Guid ownerId,
            CreateDocumentRequest request,
            CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var document = new Document
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            FileName = request.FileName.Trim(),
            ContentType = request.ContentType.Trim(),
            FileSizeBytes = request.FileSizeBytes,
            ExternalReference = request.ExternalReference?.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _dbContext.Documents.Add(document);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(document);
    }

    public async Task<DocumentResponse?> UpdateAsync(
            Guid ownerId,
            Guid documentId,
            UpdateDocumentRequest request,
            CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var document = await _dbContext.Documents
            .FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.Id == documentId, cancellationToken);

        if (document is not null)
        {
            document.Title = request.Title.Trim();
            document.Description = request.Description?.Trim();
            document.FileName = request.FileName.Trim();
            document.ContentType = request.ContentType.Trim();
            document.FileSizeBytes = request.FileSizeBytes;
            document.ExternalReference = request.ExternalReference?.Trim();
            document.UpdatedAtUtc = now;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return document is null ? null : Map(document);
    }

    public async Task<bool> DeleteAsync(
            Guid ownerId,
            Guid documentId,
            CancellationToken cancellationToken)
    {
        var document = await _dbContext.Documents
            .FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.Id == documentId, cancellationToken);

        if (document is not null)
        {
            _dbContext.Documents.Remove(document);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return document is null ? false : true;
    }

    public async Task<DocumentResponse?> GetByIdAsync(
            Guid ownerId,
            Guid documentId,
            CancellationToken cancellationToken)
    {
        var document = await _dbContext.Documents
            .AsNoTracking()
            .Where(x => x.OwnerId == ownerId && x.Id == documentId)
            .SingleOrDefaultAsync(cancellationToken);

        return document is null ? null : Map(document);
    }

    public async Task<PagedResponse<DocumentResponse>> ListAsync(
            Guid ownerId,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
    {
        var query = _dbContext.Documents
            .AsNoTracking()
            .Where(x => x.OwnerId == ownerId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new DocumentResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                FileName = x.FileName,
                ContentType = x.ContentType,
                FileSizeBytes = x.FileSizeBytes,
                ExternalReference = x.ExternalReference,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<DocumentResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private static DocumentResponse Map(Document document)
    {
        return new DocumentResponse
        {
            Id = document.Id,
            Title = document.Title,
            Description = document.Description,
            FileName = document.FileName,
            ContentType = document.ContentType,
            FileSizeBytes = document.FileSizeBytes,
            ExternalReference = document.ExternalReference,
            CreatedAtUtc = document.CreatedAtUtc,
            UpdatedAtUtc = document.UpdatedAtUtc
        };
    }
}
