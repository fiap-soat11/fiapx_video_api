namespace Infrastructure.Messaging;

public class SqsSettings
{
    public string QueueUrl { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Session_Token { get; set; } = string.Empty;
}

