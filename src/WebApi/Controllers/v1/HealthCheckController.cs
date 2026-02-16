using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers.v1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/health")]
[ApiController]
public class HealthCheckController : ApiControllerBase
{
    public HealthCheckController(MediatR.IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthCheckResponse> Get()
    {
        return Ok(new HealthCheckResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("auth-test")]
    [Authorize]
    [ProducesResponseType(typeof(AuthTestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<AuthTestResponse> AuthTest()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new AuthTestResponse
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            UserName = User.Identity?.Name ?? "Unknown",
            Claims = claims,
            Message = "Authentication successful"
        });
    }
}

public record HealthCheckResponse
{
    public string Status { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}

public record AuthTestResponse
{
    public bool IsAuthenticated { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public object? Claims { get; init; }
}

