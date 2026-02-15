namespace Application.Interfaces;

public interface IS3Service
{
    Task<string> UploadVideoAsync(Stream fileStream, string fileName, string contentType, string key, CancellationToken cancellationToken = default);
    Task<string> GeneratePresignedUrlAsync(string key, int expirationMinutes = 15, CancellationToken cancellationToken = default);
}

