using Microsoft.AspNetCore.Mvc;

namespace Anoroc_User_Management.Controllers
{
    public class LoginController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}