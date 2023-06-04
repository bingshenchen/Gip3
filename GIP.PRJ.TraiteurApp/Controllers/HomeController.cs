using GIP.PRJ.TraiteurApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GIP.PRJ.TraiteurApp.Services.Interfaces;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; 
        private readonly IBusinessHoursService _businessHoursService;

        public HomeController(ILogger<HomeController> logger, IBusinessHoursService businessHoursService)
        {
            _logger = logger;
            _businessHoursService = businessHoursService;
        }

        public async Task<IActionResult> Index()
        {
            var businessHours = await _businessHoursService.GetBusinessHours();
            return View(businessHours);
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
}