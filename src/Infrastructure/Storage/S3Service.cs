using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

public class S3Service : Application.Interfaces.IS3Service
{
    private readonly Application.Interfaces.S3Settings _settings;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IOptions<Application.Interfaces.S3Settings> settings)
    {
        _settings = settings.Value;
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_settings.Region)
        };
        _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
    }

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
}