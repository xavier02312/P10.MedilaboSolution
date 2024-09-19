using PatientRapportDiabete.Models;
using Serilog;
using System.Net.Http.Headers;

namespace PatientFront.Service
{
    public class PatientRapportDiabeteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientRapportDiabeteService> _logger;
        public PatientRapportDiabeteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientRapportDiabeteService> logger)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            // Récupérer le token JWT depuis le cookie
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        // Récupérer l'id du Patient
        public async Task<PatientModel?> Get(int id)
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync($"/Patient/get?id={id}");


                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PatientModel?>();
                }
                else
                {
                    // Log l'erreur pour plus de détails
                    Log.Error($"Request failed with status code: {response.StatusCode} and reason: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log l'exception
                Log.Error($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log toute autre exception
                Log.Error($"Unexpected error: {ex.Message}");
            }
            return null;
        }
        // Récupérer des notes
        public async Task<List<NoteModel>?> GetNote(int patientId)
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync($"/Note/GetNotes?patientId={patientId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<NoteModel>?>();
                }
                else
                {
                    // Log l'erreur pour plus de détails
                    Log.Error($"Request failed with status code: {response.StatusCode} and reason: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log l'exception
                Log.Error($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log toute autre exception
                Log.Error($"Unexpected error: {ex.Message}");
            }
            return null;
        }
        // Niveau de risque d'un patient
        public async Task<RiskEnum?> GetRiskLevel(int patientId)
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync($"/DiabeteRisk/Get?id={patientId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RiskEnum?>();
                }
                else
                {
                    Log.Error($"Request failed with status code: {response.StatusCode} and reason: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error: {ex.Message}");
            }
            return null;
        }
    }
}
