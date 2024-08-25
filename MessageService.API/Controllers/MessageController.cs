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
        private readonly IMessageRepository _repository;
        private readonly IWebSocketHandler _webSocketHandler;
        private readonly ILogger<MessageController> _logger;
        private readonly IMapper _mapper;

        public MessageController(IMessageRepository repository, IWebSocketHandler webSocketHandler, ILogger<MessageController> logger, IMapper mapper)
        {
            _repository = repository;
            _webSocketHandler = webSocketHandler;
            _logger = logger;
            _mapper = mapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the HTTP POST request to add a new message.
        /// </summary>
        /// <param name="message">The message to be added.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns
        [HttpPost("add")]
        public async Task<IActionResult> PostMessage([FromBody] Message message)
        {
            try
            {
                message.Timestamp = DateTime.UtcNow;
                await _repository.SaveMessageAsync(message);

                var messageDto = _mapper.Map<MessageDto>(message);

                await _webSocketHandler.SendMessageToAllAsync($"{messageDto.Timestamp} {messageDto.UserName}: {messageDto.Text}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MessageController.PostMessage {Message}{StackTrace}{InnerException}",
                    ex.Message,
                    ex.StackTrace,
                    ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        /// <summary>
        /// Handles the HTTP GET request to retrieve messages within a specified date range.
        /// </summary>
        /// <param name="from">The start date and time of the range for message retrieval.</param>
        /// <param name="to">The end date and time of the range for message retrieval.</param>
        /// <returns>An IActionResult representing the result of the operation, including the retrieved messages.</returns>
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
                _logger.LogError(ex, "MessageController.PostMessage {Message}{StackTrace}{InnerException}",
                    ex.Message,
                    ex.StackTrace,
                    ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
