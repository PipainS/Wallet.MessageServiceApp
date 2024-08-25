using System.Data;
using Dapper;
using Npgsql;
using MessageService.API.Models;

namespace MessageService.API.Repositories
{
    public class MessageRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(string connectionString, ILogger<MessageRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task SaveMessageAsync(Message message)
        {
            try
            {
                _logger.LogInformation("Adding message to the database: {Message}", message.Text);

                using var connection = new NpgsqlConnection(_connectionString);
                await connection.ExecuteAsync(
                    "INSERT INTO public.messages (user_name, text, timestamp, client_order) VALUES (@UserName, @Text, @Timestamp, @ClientOrder)", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding message to the database");
                throw;
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                return await connection.QueryAsync<Message>(
                    "SELECT user_name AS UserName, text AS Text, timestamp AS Timestamp, client_order AS ClientOrder FROM messages WHERE timestamp BETWEEN @From AND @To ORDER BY timestamp",
                    new { From = from, To = to });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching messages from the database");
                throw;
            }
        }

    }
}
