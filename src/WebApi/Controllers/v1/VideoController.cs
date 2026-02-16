using Application.UseCases.GetProcessedVideo;
using Application.UseCases.GetVideoStatus;
using Application.UseCases.UploadVideo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers.v1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/videos")]
[ApiController]
[Authorize]
public class VideoController : ApiControllerBase
{
    public VideoController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadVideoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<UploadVideoResponse>> UploadVideo(
        [FromForm] UploadVideoRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UploadVideoCommand(
            request.VideoFile.OpenReadStream(),
            request.VideoFile.FileName,
            request.VideoFile.ContentType ?? "video/mp4",
            request.VideoFile.Length);
        var result = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetVideoStatus), new { id = result.VideoId, version = "1.0" }, result);
    }

    [HttpGet("{id:guid}/download")]
    [ProducesResponseType(typeof(GetProcessedVideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetProcessedVideoResponse>> GetProcessedVideo(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProcessedVideoQuery(id);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetVideoStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetVideoStatusResponse>> GetVideoStatus(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetVideoStatusQuery(id);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

