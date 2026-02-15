using Microsoft.AspNetCore.Mvc;

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
}

public record HealthCheckResponse
{
    public string Status { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}

