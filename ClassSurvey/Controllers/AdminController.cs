using ClassSurvey.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassSurvey.Controllers;

public class AdminController : Controller
{
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(SignInVM viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View(viewModel);
            }
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return View("Home", "Index");
        }
    }
}
