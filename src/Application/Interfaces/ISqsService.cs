namespace Application.Interfaces;

public class VideoProcessingMessage
{
    public int VideoId { get; set; }
    public string S3Key { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public interface ISqsService
{
    Task PublishVideoProcessingMessageAsync(VideoProcessingMessage message, CancellationToken cancellationToken = default);
}

