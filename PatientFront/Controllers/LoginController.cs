using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientService.Models;

namespace PatientFront.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthenticationLogin _authenticationLogin;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(AuthenticationLogin authenticationLogin, IHttpContextAccessor httpContextAccessor)
        {
            _authenticationLogin = authenticationLogin;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _authenticationLogin.Login(model.Username, model.Password);
                if (token != null)
                {
                    // JWT dans le header
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(1) // Définir la durée de vie du cookie
                    };
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("Jwt", token, cookieOptions);

                    // Message de connexion avec le nom de l'utilisateur
                    TempData["SuccessMessage"] = $"Connexion réussie ! Bienvenue, { model.Username}.";

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            // Récupérer le nom de l'utilisateur à partir des revendications
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Utilisateur";

            HttpContext.Session.Clear();

            // Supprimer le cookie JWT
            if (HttpContext.Request.Cookies["Jwt"] != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-1)
                };
                HttpContext.Response.Cookies.Append("Jwt", "", cookieOptions);
            }

            TempData["SuccessMessage"] = $"Déconnexion réussie, {userName}.";

            return RedirectToAction("Index", "Home");
        }
    }
}
