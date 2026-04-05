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

        var document = await _documentService.GetByIdAsync(userId, id, cancellationToken);

        return document is null ? NotFound() : Ok(document);
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

    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserId = _currentUserService.GetUserId(),
            Email = _currentUserService.GetEmail()
        });
    }
}
