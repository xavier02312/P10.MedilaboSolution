using PatientService.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PatientFront.Service
{
    public class AuthenticationLogin : IAuthenticationServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationLogin> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationLogin(HttpClient httpClient, ILogger<AuthenticationLogin> logger, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        // Connexion
        public async Task<string> Login(string username, string password)
        {
            try
            {
                // Effectuer la requête POST
                var connection = await _httpClient.PostAsJsonAsync("/Authentication/Login", 
                    new { Username = username, Password = password });
                connection.EnsureSuccessStatusCode();

                var responseContent = await connection.Content.ReadAsStringAsync();

                var tokenObject = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                var token = tokenObject["value"];

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
            }

            return string.Empty;
        }
        // Déconnexion
        public async Task<string> GetUserNameFromTokenAsync(string token)
        {
            return await Task.Run(() =>
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userNameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name");

                return userNameClaim?.Value ?? "Utilisateur";
            });
        }
    }
}
