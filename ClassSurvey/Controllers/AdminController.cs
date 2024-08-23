using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
