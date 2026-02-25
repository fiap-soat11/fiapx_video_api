using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.UseCases.UploadVideo;
public sealed class UploadVideoCommandHandler(IS3Service s3Service, ISqsService sqsService, IVideoRepository videoRepository, IUserRepository userRepository, IOptions<S3Settings> s3Settings) : IRequestHandler<UploadVideoCommand, UploadVideoResponse>
{
    public async Task<UploadVideoResponse> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        //var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        //if (!userExists)
        //    throw new KeyNotFoundException($"User with id {request.UserId} not found. Create the user before uploading a video.");

        var prefix = Guid.NewGuid();
        var fileName = $"{prefix}{Path.GetExtension(request.FileName)}";
        var s3Key = $"videos/{prefix}/original/{fileName}";

        await s3Service.UploadVideoAsync(request.VideoStream, request.FileName, request.ContentType, s3Key, cancellationToken);

        var videoProcessing = new VideoProcessing
        {
            UserId = request.UserId,
            OriginalFileName = request.FileName,
            Status = "Pending",
            S3InputPath = s3Key,
            CreatedAt = DateTime.UtcNow
        };

        await videoRepository.AddAsync(videoProcessing, cancellationToken);

        var message = new VideoProcessingMessage
        {
            VideoId = videoProcessing.Id,
            S3Key = s3Key,
            BucketName = s3Settings.Value.BucketName
        };
        await sqsService.PublishVideoProcessingMessageAsync(message, cancellationToken);

        return new UploadVideoResponse
        {
            Id = videoProcessing.Id,
            Status = videoProcessing.Status,
            CreatedAt = videoProcessing.CreatedAt
        };
    }
}