using Microsoft.AspNetCore.Mvc;

namespace MessageClient3.Controllers
{
    public class MessagesController : Controller
    {
        private readonly HttpClient _httpClient;

        public MessagesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MessageApiClient");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                var now = DateTime.UtcNow;
                var from = now.AddMinutes(-10);
                var to = now;

                var response = await _httpClient.GetAsync($"api/messages/get?from={from:O}&to={to:O}");
                response.EnsureSuccessStatusCode();

                var messages = await response.Content.ReadAsStringAsync();
                return Json(messages);
            }
            catch (Exception)
            {
                // Log the error and handle accordingly
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
