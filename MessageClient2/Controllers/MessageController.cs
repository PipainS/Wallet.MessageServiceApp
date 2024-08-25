using Microsoft.AspNetCore.Mvc;

namespace MessageClient2.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
