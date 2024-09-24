using Microsoft.AspNetCore.Mvc;
using PatientFront.Models;
using Serilog;
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
            try
            {
                if (statusCode == 401)
                {
                    return await Task.FromResult(View("401"));
                }
                else if (statusCode == 403)
                {
                    return await Task.FromResult(View("403"));
                }
                else if (statusCode == 404)
                {
                    return await Task.FromResult(View("404"));
                }

                return await Task.FromResult(View("Error"));
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Log.Error(ex, "An error occurred while processing the error page request.");

                // Return a generic error view
                return await Task.FromResult(View("Error"));
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.FromResult(View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }
    }
}
