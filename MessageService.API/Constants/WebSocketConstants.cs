namespace MessageService.API.Constants
{
    /// <summary>
    /// Contains constant values used for WebSocket operations.
    /// </summary>
    public class WebSocketConstants
    {
        /// <summary>
        /// The starting position in the buffer array from which bytes are read to be converted into a string.
        /// </summary>
        public const int BufferStartIndex = 0;

        /// <summary>
        /// The size of the buffer used for receiving data via WebSocket (4 KB).
        /// </summary>
        public const int BufferSize = 1024 * 4;
    }
}
