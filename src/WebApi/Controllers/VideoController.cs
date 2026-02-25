using Application.UseCases.GetProcessedVideo;
using Application.UseCases.GetVideoStatus;
using Application.UseCases.UploadVideo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("videos")]
[ApiController]
//[Authorize]
public class VideoController(IMediator mediator) : ApiControllerBase(mediator)
{
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
            request.UserId,
            request.VideoFile.OpenReadStream(),
            request.VideoFile.FileName,
            request.VideoFile.ContentType ?? "video/mp4",
            request.VideoFile.Length);
        var result = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetVideoStatus), new { id = result.Id }, result);
    }

    [HttpGet("{id:int}/download")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetProcessedVideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProcessedVideo(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetProcessedVideoQuery(id);
        var result = await Mediator.Send(query, cancellationToken);

        if (result.FileStream != null)
            return File(result.FileStream, result.ContentType ?? "application/octet-stream", result.FileName ?? "download.zip");

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetVideoStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetVideoStatusResponse>> GetVideoStatus(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetVideoStatusQuery(id);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

