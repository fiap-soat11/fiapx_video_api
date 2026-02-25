using Amazon.SQS;
using Amazon.SQS.Model;
using Application.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Messaging;

public class SqsService(IOptions<SqsSettings> settings) : Application.Interfaces.ISqsService
{
    private readonly SqsSettings _settings = settings.Value;
    private readonly IAmazonSQS _sqsClient = new AmazonSQSClient(
        settings.Value.AccessKey, 
        settings.Value.SecretKey,
        settings.Value.Session_Token,
        new AmazonSQSConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(settings.Value.Region)
        });

    public async Task PublishVideoProcessingMessageAsync(Application.Interfaces.VideoProcessingMessage message, CancellationToken cancellationToken = default)
    {
        var messageBody = JsonSerializer.Serialize(message);
        var messageDeduplicationId = GenerateMessageDeduplicationId(message);
        
        var request = new SendMessageRequest
        {
            QueueUrl = _settings.QueueUrl,
            MessageBody = messageBody,
            MessageGroupId = message.VideoId.ToString(),
            MessageDeduplicationId = messageDeduplicationId
        };

        await _sqsClient.SendMessageAsync(request, cancellationToken);
    }

    private static string GenerateMessageDeduplicationId(Application.Interfaces.VideoProcessingMessage message)
    {
        var content = $"{message.VideoId}_{message.S3Key}_{message.Timestamp:O}";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
        var base64Hash = Convert.ToBase64String(hashBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
        return base64Hash.Length > 128 ? base64Hash.Substring(0, 128) : base64Hash;
    }
}

