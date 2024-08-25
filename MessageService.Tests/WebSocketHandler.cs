//using System.Net.WebSockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//public class WebSocketHandlerTests
//{
//    [Fact]
//    public async Task HandleWebSocketConnection_ShouldAcceptWebSocketAndReceiveMessages()
//    {
//        // Arrange
//        var connectionManager = new Mock<WebSocketConnectionManager>();
//        var logger = new Mock<ILogger<WebSocketHandler>>();
//        var handler = new WebSocketHandler(connectionManager.Object, logger.Object);

//        var httpContext = new DefaultHttpContext();
//        var webSocket = new Mock<WebSocket>();

//        // Mock the AcceptWebSocketAsync method
//        var acceptWebSocketAsync = Task.FromResult(webSocket.Object);
//        httpContext.WebSockets = new Mock<WebSocketCollection>(acceptWebSocketAsync).Object;

//        var buffer = Encoding.UTF8.GetBytes("Test Message");
//        var receiveResult = new WebSocketReceiveResult(buffer.Length, WebSocketMessageType.Text, true);

//        webSocket.Setup(s => s.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
//                  .ReturnsAsync(receiveResult);
//        webSocket.Setup(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, CancellationToken.None))
//                  .Returns(Task.CompletedTask);

//        connectionManager.Setup(cm => cm.AddSocket(It.IsAny<WebSocket>())).Returns("socketId");
//        connectionManager.Setup(cm => cm.GetSocketById(It.IsAny<string>())).Returns(webSocket.Object);

//        // Act
//        await handler.HandleWebSocketConnection(httpContext);

//        // Assert
//        connectionManager.Verify(cm => cm.AddSocket(It.IsAny<WebSocket>()), Times.Once);
//        webSocket.Verify(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, CancellationToken.None), Times.Once);
//    }
//}
