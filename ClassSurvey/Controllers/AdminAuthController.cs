using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey.Controllers
{
    public class AdminAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
