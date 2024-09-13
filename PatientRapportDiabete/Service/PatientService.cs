using PatientRapportDiabete.Models;
using Serilog;
using System.Net.Http.Headers;

namespace PatientRapportDiabete.Service
{
    public class PatientService
    {
        private readonly HttpClient _httpClient;
        public PatientService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7234");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
        }
        public async Task<PatientModel?> Get(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

            try
            {
                // Effectuer la requête GET
                HttpResponseMessage response = await _httpClient.GetAsync($"/gateway/Patient/get?id={id}");

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
    }
}