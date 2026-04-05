using DocumentMetadataService.Api.Contracts;
using DocumentMetadataService.Api.Domain;

namespace DocumentMetadataService.Api.Data;

public interface IDocumentService
{
    Task<DocumentResponse> CreateAsync(
            Guid ownerId,
            CreateDocumentRequest request,
            CancellationToken cancellationToken);

    Task<DocumentResponse?> GetByIdAsync(
            Guid ownerId,
            Guid documentId,
            CancellationToken cancellationToken);

    Task<PagedResponse<DocumentResponse>> ListAsync(
            Guid ownerId,
            int page,
            int pageSize,
            CancellationToken cancellationToken);
}
