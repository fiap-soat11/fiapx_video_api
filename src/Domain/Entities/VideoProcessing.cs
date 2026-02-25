namespace Domain.Entities;

public partial class VideoProcessing
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Failed

    public string S3InputPath { get; set; } = null!;

    public string? S3OutputPath { get; set; }

    public string? FailureReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
