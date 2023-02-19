using Microsoft.AspNetCore.Mvc;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChatController : Controller
    {
 
        public IActionResult Index()
        {
            return View();
        }
    }
}
