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
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Login(string username, string password)
        {
            try
            {
                var connection = await _httpClient.PostAsJsonAsync("/Authentication/Login", 
                    new { Username = username, Password = password });
                connection.EnsureSuccessStatusCode();
                
                var responseContent = await connection.Content.ReadAsStringAsync();

                if (responseContent != null && !string.IsNullOrEmpty(responseContent))
                {
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
