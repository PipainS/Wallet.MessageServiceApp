using MessageService.API.Models;
using MessageService.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Json;

public class MessageControllerTests
{
    private readonly TestServer _server;
    private readonly HttpClient _client;

    public MessageControllerTests()
    {
        var webSocketHandlerMock = new Mock<WebSocketHandler>();
        var messageRepositoryMock = new Mock<MessageRepository>();

        _server = new TestServer(new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddControllers();
                services.AddSingleton<WebSocketConnectionManager>();
                services.AddSingleton<WebSocketHandler>(); ;
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }));
        _client = _server.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5103"); // Set the base address for HttpClient
    }

    [Fact]
    public async Task PostMessage_ShouldReturnOk()
    {
        // Arrange
        var message = new Message
        {
            UserName = "User",
            Text = "Hello World",
            Id = 1,  // Provide appropriate values for properties required by your API
            Timestamp = DateTime.UtcNow,
            ClientOrder = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/messages/add", message);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetMessages_ShouldReturnMessages()
    {
        // Act
        var response = await _client.GetAsync("/api/messages/get?from=2024-01-01T00:00:00Z&to=2024-12-31T23:59:59Z");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }
}
