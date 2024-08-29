using PatientService.Models;
using PatientService.Service;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PatientFront.Service
{
    public class AuthenticationLogin : IAuthenticationServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationLogin> _logger;

        public AuthenticationLogin(HttpClient httpClient, ILogger<AuthenticationLogin> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<string> Login(string username, string password)
        {
            try
            {
                var loginModel = new LoginModel { Username = username, Password = password };
                var connection = await _httpClient.PostAsJsonAsync("/Authentication/Login", loginModel);
                connection.EnsureSuccessStatusCode();

                var responseContent = await connection.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
                    return tokenResponse.Token;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
            }

            return string.Empty;
        }
        public class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
