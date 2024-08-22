using Microsoft.AspNetCore.Mvc;
using MessageService.API.Models;
using MessageService.API.Repositories;

namespace MessageService.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly MessageRepository _repository;
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ILogger<MessageController> _logger;

        public MessageController(MessageRepository repository, WebSocketHandler webSocketHandler, ILogger<MessageController> logger)
        {
            _repository = repository;
            _webSocketHandler = webSocketHandler;
            _logger = logger;
        }

        [HttpPost("api/messages")]
        public async Task<IActionResult> PostMessage([FromBody] Message message)
        {
            try
            {
                message.Timestamp = DateTime.UtcNow;
                await _repository.SaveMessageAsync(message);

                // Отправляем сообщение по WebSocket всем подключенным клиентам
                await _webSocketHandler.SendMessageToAllAsync($"{message.Timestamp}: {message.Text}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing message");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var messages = await _repository.GetMessagesAsync(from, to);
            return Ok(messages);
        }
    }
}
