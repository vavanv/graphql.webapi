using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserFullName = $"{User.FindFirst("FirstName")?.Value} {User.FindFirst("LastName")?.Value}";
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
