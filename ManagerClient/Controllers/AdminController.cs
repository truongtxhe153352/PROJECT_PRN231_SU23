using Microsoft.AspNetCore.Mvc;

namespace ManagerClient.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
