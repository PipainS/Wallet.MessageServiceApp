using System.Net.WebSockets;
using Moq;

namespace MessageService.Tests
{
    public class WebSocketConnectionManagerTests
    {
        [Fact]
        public void AddSocket_ShouldAddSocketAndReturnId()
        {
            // Arrange
            var manager = new WebSocketConnectionManager();
            var socket = new Mock<WebSocket>();

            // Act
            var id = manager.AddSocket(socket.Object);

            // Assert
            Assert.NotNull(id);
            Assert.Equal(socket.Object, manager.GetSocketById(id));
        }

        [Fact]
        public async Task RemoveSocket_ShouldCloseAndDisposeSocket()
        {
            // Arrange
            var manager = new WebSocketConnectionManager();
            var socket = new Mock<WebSocket>();
            var id = manager.AddSocket(socket.Object);

            socket.Setup(s => s.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                  .Returns(Task.CompletedTask);

            // Act
            await manager.RemoveSocket(id);

            // Assert
            Assert.Null(manager.GetSocketById(id));
            socket.Verify(s => s.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            socket.Verify(s => s.Dispose(), Times.Once);
        }
    }
}