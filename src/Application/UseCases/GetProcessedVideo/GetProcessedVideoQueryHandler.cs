using Application.Interfaces;
using Application.Repositories;
using MediatR;

namespace Application.UseCases.GetProcessedVideo;

public sealed class GetProcessedVideoQueryHandler(
    IVideoRepository videoRepository,
    Application.Interfaces.IS3Service s3Service) : IRequestHandler<GetProcessedVideoQuery, GetProcessedVideoResponse>
{
    public async Task<GetProcessedVideoResponse> Handle(GetProcessedVideoQuery request, CancellationToken cancellationToken)
    {
        var video = await videoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (video == null)
        {
            throw new KeyNotFoundException($"Video with id {request.Id} not found.");
        }

        string? downloadUrl = null;
        DateTime? expiresAt = null;
        Stream? fileStream = null;
        string? fileName = null;
        string? contentType = null;

        if (video.Status == "Completed" && !string.IsNullOrEmpty(video.S3OutputPath))
        {
            var (stream, contentTypeFromS3) = await s3Service.GetObjectStreamAsync(video.S3OutputPath, cancellationToken);
            fileStream = stream;
            contentType = contentTypeFromS3;
            fileName = Path.ChangeExtension(video.OriginalFileName, ".zip") ?? "processed.zip";
            downloadUrl = await s3Service.GeneratePresignedUrlAsync(video.S3OutputPath, 15, cancellationToken);
            expiresAt = DateTime.UtcNow.AddMinutes(15);
        }

        return new GetProcessedVideoResponse
        {
            Id = video.Id,
            Status = video.Status,
            DownloadUrl = downloadUrl,
            ExpiresAt = expiresAt,
            FileStream = fileStream,
            FileName = fileName,
            ContentType = contentType
        };
    }

}

