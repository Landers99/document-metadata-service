using DocumentMetadataService.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentMetadataService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController : ControllerBase
{
    [HttpGet("db-check")]
    public async Task<IActionResult> CheckDatabase([FromServices] AppDbContext dbContext)
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        return Ok(new { databaseConnected = canConnect });
    }
}
