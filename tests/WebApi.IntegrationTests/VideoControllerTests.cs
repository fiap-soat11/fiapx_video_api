using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using NUnit.Framework;

namespace WebApi.IntegrationTests;

[TestFixture]
public class VideoControllerTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task UploadVideo_ValidFile_ReturnsCreated()
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3, 4 });
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
        content.Add(fileContent, "videoFile", "test-video.mp4");

        // Add JWT token for authentication
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6WyJhZG1pbkB0ZXN0ZS5jb20iLCJhZG1pbkB0ZXN0ZS5jb20iXSwidW5pcXVlX25hbWUiOiJhZG1pbkB0ZXN0ZS5jb20iLCJzdWIiOiJhZG1pbkB0ZXN0ZS5jb20iLCJqdGkiOiI0ZTU5OWM5YS0wMTg0LTQ0ZjYtYmMzZC0zYjFjMTYxMDRkYjEiLCJpYXQiOjE3NzExNzA3NjAsIm5iZiI6MTc3MTE3MDc2MCwiZXhwIjoxNzcxMTc0MzYwLCJpc3MiOiJmaWFweF91c3VhcmlvX3NlcnZpY2UiLCJhdWQiOiJmaWFweF91c3VhcmlvX2NsaWVudCJ9.6ZMM8nrv0y25DZpHpXjGUyVTI9K0KL9tKp0F8chn3mI");

        var response = await _client.PostAsync("/videos", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task UploadVideo_InvalidFile_ReturnsBadRequest()
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3, 4 });
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        content.Add(fileContent, "videoFile", "test.pdf");

        // Add JWT token for authentication
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6WyJhZG1pbkB0ZXN0ZS5jb20iLCJhZG1pbkB0ZXN0ZS5jb20iXSwidW5pcXVlX25hbWUiOiJhZG1pbkB0ZXN0ZS5jb20iLCJzdWIiOiJhZG1pbkB0ZXN0ZS5jb20iLCJqdGkiOiI0ZTU5OWM5YS0wMTg0LTQ0ZjYtYmMzZC0zYjFjMTYxMDRkYjEiLCJpYXQiOjE3NzExNzA3NjAsIm5iZiI6MTc3MTE3MDc2MCwiZXhwIjoxNzcxMTc0MzYwLCJpc3MiOiJmaWFweF91c3VhcmlvX3NlcnZpY2UiLCJhdWQiOiJmaWFweF91c3VhcmlvX2NsaWVudCJ9.6ZMM8nrv0y25DZpHpXjGUyVTI9K0KL9tKp0F8chn3mI");

        var response = await _client.PostAsync("/videos", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetVideoStatus_NonExistentId_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();

        // Add JWT token for authentication
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6WyJhZG1pbkB0ZXN0ZS5jb20iLCJhZG1pbkB0ZXN0ZS5jb20iXSwidW5pcXVlX25hbWUiOiJhZG1pbkB0ZXN0ZS5jb20iLCJzdWIiOiJhZG1pbkB0ZXN0ZS5jb20iLCJqdGkiOiI0ZTU5OWM5YS0wMTg0LTQ0ZjYtYmMzZC0zYjFjMTYxMDRkYjEiLCJpYXQiOjE3NzExNzA3NjAsIm5iZiI6MTc3MTE3MDc2MCwiZXhwIjoxNzcxMTc0MzYwLCJpc3MiOiJmaWFweF91c3VhcmlvX3NlcnZpY2UiLCJhdWQiOiJmaWFweF91c3VhcmlvX2NsaWVudCJ9.6ZMM8nrv0y25DZpHpXjGUyVTI9K0KL9tKp0F8chn3mI");

        var response = await _client.GetAsync($"/videos/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

