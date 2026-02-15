namespace Domain.Entities;

public class Video : IAggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string FileName { get; private set; } = string.Empty;
    public string OriginalFileName { get; private set; } = string.Empty;
    public string S3Key { get; private set; } = string.Empty;
    public string? S3ZipKey { get; private set; }
    public VideoStatus Status { get; private set; }
    public long FileSize { get; private set; }
    public string ContentType { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    private Video()
    {
    }

    public static Video Create(
        string fileName,
        string originalFileName,
        string s3Key,
        long fileSize,
        string contentType)
    {
        return new Video
        {
            FileName = fileName,
            OriginalFileName = originalFileName,
            S3Key = s3Key,
            FileSize = fileSize,
            ContentType = contentType,
            Status = VideoStatus.Uploaded
        };
    }

    public void MarkAsProcessing()
    {
        Status = VideoStatus.Processing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsProcessed(string s3ZipKey)
    {
        Status = VideoStatus.Processed;
        S3ZipKey = s3ZipKey;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = VideoStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }
}

