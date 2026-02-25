using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

public class S3Service(IOptions<Application.Interfaces.S3Settings> settings) : Application.Interfaces.IS3Service
{
    private readonly Application.Interfaces.S3Settings _settings = settings.Value;
    private readonly IAmazonS3 _s3Client = new AmazonS3Client(
        settings.Value.AccessKey, 
        settings.Value.SecretKey,
        settings.Value.Session_Token,
        new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(settings.Value.Region)
        });

    public async Task<string> UploadVideoAsync(Stream fileStream, string fileName, string contentType, string key, CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
        return key;
    }

    public async Task<string> GeneratePresignedUrlAsync(string key, int expirationMinutes = 15, CancellationToken cancellationToken = default)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };

        return await Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task<(Stream Stream, string ContentType)> GetObjectStreamAsync(string key, CancellationToken cancellationToken = default)
    {
        var request = new GetObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key
        };
        var response = await _s3Client.GetObjectAsync(request, cancellationToken);
        var contentType = response.Headers.ContentType ?? "application/octet-stream";
        return (response.ResponseStream, contentType);
    }
}