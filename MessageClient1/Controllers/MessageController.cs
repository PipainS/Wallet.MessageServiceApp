using Microsoft.AspNetCore.Mvc;
using MessageClient1.Models;
using System.Text;
using Newtonsoft.Json;

namespace MessageClient1.Controllers
{
    public class MessageController : Controller
    {
        private readonly HttpClient _httpClient;

        public MessageController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5103");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(Message message)
        {
            if (ModelState.IsValid)
            {
                message.Timestamp = DateTime.UtcNow;
                var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/messages/add", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["MessageStatus"] = "Message sent successfully.";
                }
                else
                {
                    ViewData["MessageStatus"] = "Failed to send message.";
                }
            }
            else
            {
                ViewData["MessageStatus"] = "Failed to send message.";
            }

            return View("Index");
        }
    }
}
