using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DocumentMetadataService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DebugController : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new
        {
            Message = "Token is valid",
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

}
