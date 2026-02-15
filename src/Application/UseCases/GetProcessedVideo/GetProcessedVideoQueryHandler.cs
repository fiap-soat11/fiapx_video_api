using Application.Interfaces;
using Application.Repositories;
using MediatR;

namespace Application.UseCases.GetProcessedVideo;

public sealed class GetProcessedVideoQueryHandler : IRequestHandler<GetProcessedVideoQuery, GetProcessedVideoResponse>
{
    private readonly IVideoRepository _videoRepository;
    private readonly Application.Interfaces.IS3Service _s3Service;

    public GetProcessedVideoQueryHandler(
        IVideoRepository videoRepository,
        Application.Interfaces.IS3Service s3Service)
    {
        _videoRepository = videoRepository;
        _s3Service = s3Service;
    }

    public async Task<GetProcessedVideoResponse> Handle(GetProcessedVideoQuery request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId, cancellationToken);

        if (video == null)
        {
            throw new KeyNotFoundException($"Video with id {request.VideoId} not found.");
        }

        string? downloadUrl = null;
        DateTime? expiresAt = null;

        if (video.Status == Domain.Entities.VideoStatus.Processed && !string.IsNullOrEmpty(video.S3ZipKey))
        {
            downloadUrl = await _s3Service.GeneratePresignedUrlAsync(video.S3ZipKey, 15, cancellationToken);
            expiresAt = DateTime.UtcNow.AddMinutes(15);
        }

        return new GetProcessedVideoResponse
        {
            VideoId = video.Id,
            Status = video.Status,
            DownloadUrl = downloadUrl,
            ExpiresAt = expiresAt
        };
    }
}

