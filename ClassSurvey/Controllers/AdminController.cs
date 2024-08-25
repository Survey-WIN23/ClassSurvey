using ClassSurvey.Entities;
using ClassSurvey.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassSurvey.Controllers;

public class AdminController(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, SignInManager<UserEntity> signInManager) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    [HttpGet]
    public IActionResult SignIn()
    {
        if (!_userManager.Users.Any(u => u.UserName == "ted.pieplow@gmail.com"))
        {
            ViewBag.ShowSetup = true;
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInVM viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Form.Email, viewModel.Form.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "BackOffice"); 
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login attempt";
                return View(viewModel);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return View("Home", "Index");
        }
    }

    [HttpGet]
    [Route("/signout")]
    public new async Task<IActionResult> SignOut()
    {
        try
        {
            Response.Cookies.Delete("AccessToken");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return NoContent();
        }
    }

    [HttpGet]
    [Authorize(Roles = "SuperUser")]
    public IActionResult SetupAdmin()
    {
        try
        {
            if (_userManager.Users.Any(u => u.UserName == "ted.pieplow@gmail.com"))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> SetupAdmin(string adminEmail, string adminPassword)
    {
        if (ModelState.IsValid)
        {
            if (!await _roleManager.RoleExistsAsync("SuperUser"))
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperUser"));
            }

            var adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "SuperUser");
                return RedirectToAction("SignIn");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        ViewBag.ShowSetup = true;
        return View("SignIn");
    }
}
