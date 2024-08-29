using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientService.Models;

namespace PatientFront.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationLogin _authenticationLogin;

        public AuthenticationController(AuthenticationLogin authenticationLogin)
        {
            _authenticationLogin = authenticationLogin;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var token = await _authenticationLogin.Login(loginModel.Username, loginModel.Password);

            if (!string.IsNullOrEmpty(token))
            {
                // Stocker le token dans un cookie ou une session
                HttpContext.Session.SetString("Jwt", token);

                return RedirectToAction("Index", "Patients");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginModel);
        }
    }
}
