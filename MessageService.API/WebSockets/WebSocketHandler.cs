using MessageService.API.Constants;
using MessageService.API.Repositories;
using System.Net.WebSockets;
using System.Text;

public class WebSocketHandler : IWebSocketHandler
{
    private readonly WebSocketConnectionManager _connectionManager;
    private readonly ILogger<WebSocketHandler> _logger;

    public WebSocketHandler(WebSocketConnectionManager connectionManager, ILogger<WebSocketHandler> logger)
    {
        _connectionManager = connectionManager;
        _logger = logger;
    }

    /// inheritdoc /
    public async Task HandleWebSocketConnection(HttpContext context)
    {
        try
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                _logger.LogInformation("WebSocket connection established");

                var socket = await context.WebSockets.AcceptWebSocketAsync();
                var socketId = _connectionManager.AddSocket(socket);

                await ReceiveMessages(socket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, WebSocketConstants.BufferStartIndex, result.Count);
                        await SendMessageToAllAsync(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _connectionManager.RemoveSocket(socketId);
                    }
                });
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocketHandler.HandleWebSocketConnection: {Message}{StackTrace}{InnerException}",
                ex.Message,
                ex.StackTrace,
                ex.InnerException?.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

    /// <summary>
    /// Receives messages from a WebSocket connection and processes them using the provided handler.
    /// </summary>
    /// <param name="socket">The WebSocket connection from which messages are received.</param>
    /// <param name="handleMessage">The action to process received messages.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task ReceiveMessages(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        var buffer = new byte[WebSocketConstants.BufferSize];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocketHandler.ReceiveMessages: {Message}{StackTrace}{InnerException}",
                ex.Message,
                ex.StackTrace,
                ex.InnerException?.Message);
        }
    }

    /// inheritdoc /
    public async Task SendMessageToAllAsync(string message)
    {
        try
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            foreach (var socket in _connectionManager.GetAll().Values)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(buffer, WebSocketConstants.BufferStartIndex, buffer.Length), 
                        WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocketHandler.SendMessageToAllAsync: {Message}{StackTrace}{InnerException}",
                ex.Message, 
                ex.StackTrace, 
                ex.InnerException?.Message);
        }
    }
}
