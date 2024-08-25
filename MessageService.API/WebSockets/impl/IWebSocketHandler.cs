namespace MessageService.API.Repositories
{
    /// <summary>
    /// Defines the contract for handling WebSocket connections and messaging.
    /// </summary>
    public interface IWebSocketHandler
    {
        /// <summary>
        /// Handles a WebSocket connection, accepting the connection and processing incoming messages.
        /// </summary>
        /// <param name="context">The HTTP context that contains the WebSocket connection request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task HandleWebSocketConnection(HttpContext context);

        /// <summary>
        /// Sends a message to all connected WebSocket clients.
        /// </summary>
        /// <param name="message">The message to be sent to all connected WebSocket clients.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendMessageToAllAsync(string message);
    }
}
