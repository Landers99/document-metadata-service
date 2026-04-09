using DocumentMetadataService.Api.Auth;
using DocumentMetadataService.Api.Contracts;
using DocumentMetadataService.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentMetadataService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ICurrentUserService _currentUserService;

    public DocumentsController(
            IDocumentService documentService,
            ICurrentUserService currentUserService)
    {
        _documentService = documentService;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public async Task<ActionResult<DocumentResponse>> Create(
            [FromBody] CreateDocumentRequest request,
            CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var response = await _documentService.CreateAsync(userId, request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DocumentResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var response = await _documentService.GetByIdAsync(userId, id, cancellationToken);

        return response is null ? NotFound() : Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DocumentResponse>> Update(
            [FromBody] UpdateDocumentRequest request,
            Guid id,
            CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var response = await _documentService.UpdateAsync(userId, id, request, cancellationToken);

        return response is null ? NotFound(new { error = "Document not found." }) : Ok(response);
    }

    [HttpDelete("id:guid")]
    public async Task<ActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var success = await _documentService.DeleteAsync(userId, id, cancellationToken);

        return (success) ? NoContent() : NotFound(new { error = "Document not found." });
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<DocumentResponse>>> List(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.GetUserId();

        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var response = await _documentService.ListAsync(userId, page, pageSize, cancellationToken);

        return Ok(response);
    }
}
