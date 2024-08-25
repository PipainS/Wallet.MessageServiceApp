    using Microsoft.AspNetCore.Mvc;
    using MessageService.API.Models;
    using MessageService.API.Repositories;
    using MessageService.API.Models.DTOs;
    using AutoMapper;

    namespace MessageService.API.Controllers
    {
        [ApiController]
        [Route("api/messages")]
        public class MessageController : ControllerBase
        {
            private readonly MessageRepository _repository;
            private readonly WebSocketHandler _webSocketHandler;
            private readonly ILogger<MessageController> _logger;
            private readonly IMapper _mapper;

            public MessageController(MessageRepository repository, WebSocketHandler webSocketHandler, ILogger<MessageController> logger, IMapper mapper)
            {
                _repository = repository;
                _webSocketHandler = webSocketHandler;
                _logger = logger;
                _mapper = mapper;
                _mapper = mapper;
            }

            [HttpPost("add")]
            public async Task<IActionResult> PostMessage([FromBody] Message message)
            {
                try
                {
                    message.Timestamp = DateTime.UtcNow;
                    await _repository.SaveMessageAsync(message);

                    string userName = string.IsNullOrWhiteSpace(message.UserName) ? "Аноним" : message.UserName;
                    // Отправляем сообщение по WebSocket всем подключенным клиентам
                    await _webSocketHandler.SendMessageToAllAsync($"{message.Timestamp} {userName}: {message.Text}");


                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"MessageController.PostMessage: {ex.Message}{ex.StackTrace}{ex.InnerException?.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
                }
            }

            [HttpGet("get")]
            public async Task<IActionResult> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to)
            {
                try
                {
                    var messages = await _repository.GetMessagesAsync(from, to);

                    var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

                    return Ok(messageDtos);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"MessageController.GetMessages: {ex.Message}{ex.StackTrace}{ex.InnerException?.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
                }
            }
        }
    }
