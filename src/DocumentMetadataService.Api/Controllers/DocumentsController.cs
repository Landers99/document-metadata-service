using DocumentMetadataService.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentMetadataService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DocumentsController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public DocumentsController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
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
