using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.UseCases.UploadVideo;
public sealed class UploadVideoCommandHandler(IS3Service s3Service, ISqsService sqsService, IVideoRepository videoRepository, IOptions<S3Settings> s3Settings) : IRequestHandler<UploadVideoCommand, UploadVideoResponse>
{
    public async Task<UploadVideoResponse> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        var videoId = Guid.NewGuid();
        var fileName = $"{videoId}{Path.GetExtension(request.FileName)}";
        var s3Key = $"videos/{videoId}/original/{fileName}";

        await s3Service.UploadVideoAsync(request.VideoStream, request.FileName, request.ContentType, s3Key, cancellationToken);

        var video = Video.Create(
            fileName,
            request.FileName,
            s3Key,
            request.FileSize,
            request.ContentType);

        await videoRepository.AddAsync(video, cancellationToken);

        var message = new VideoProcessingMessage
        {
            VideoId = video.Id,
            S3Key = s3Key,
            BucketName = s3Settings.Value.BucketName
        };
        await sqsService.PublishVideoProcessingMessageAsync(message, cancellationToken);

        return new UploadVideoResponse
        {
            VideoId = video.Id,
            Status = video.Status,
            UploadedAt = video.CreatedAt
        };
    }
}