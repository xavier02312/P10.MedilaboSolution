using Microsoft.AspNetCore.Mvc;
using PatientFront.Models;
using System.Diagnostics;

namespace PatientFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Home/Error/{statusCode}")]
        public async Task<IActionResult> Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return await Task.FromResult(View("404"));
            }
            else if (statusCode == 401 || statusCode == 403)
            {
                return await Task.FromResult(View("404"));
            }

            return await Task.FromResult(View("Error"));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.FromResult(View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }
    }
}
