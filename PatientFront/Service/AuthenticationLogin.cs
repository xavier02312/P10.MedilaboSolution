using PatientService.Models;
using PatientService.Service;

namespace PatientFront.Service
{
    public class AuthenticationLogin : IAuthenticationServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationLogin> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationLogin(HttpClient httpClient, ILogger<AuthenticationLogin> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7239");
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Login(string username, string password)
        {
            try
            {
                /*var loginModel = new LoginModel { Username = username, Password = password };*/

                var connection = await _httpClient.PostAsJsonAsync("/Authentication/Login", new { Username = username, Password = password }/*loginModel*/);
                connection.EnsureSuccessStatusCode();
                
                var responseContent = await connection.Content.ReadAsStringAsync();

                if (responseContent != null && !string.IsNullOrEmpty(responseContent))
                {
                    // Stocker le token dans un cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(7) // Définir la durée de vie du cookie
                    };
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("Jwt", responseContent, cookieOptions);

                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
            }

            return string.Empty;
        }
    }
}
