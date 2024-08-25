using MessageService.API.Models;

namespace MessageService.API.Repositories
{
    /// <summary>
    /// Defines the contract for message repository operations.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Asynchronously saves a message to the repository.
        /// </summary>
        /// <param name="message">The message to be saved.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SaveMessageAsync(Message message);

        // <summary>
        /// Asynchronously retrieves a collection of messages from the repository within the specified date range.
        /// </summary>
        /// <param name="from">The start date and time for the range of messages to retrieve.</param>
        /// <param name="to">The end date and time for the range of messages to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of messages.</returns>
        Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to);
    }
}
