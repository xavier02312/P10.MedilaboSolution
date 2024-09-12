using PatientRapportDiabete.Models;
using Serilog;
using System.Net.Http.Headers;

namespace PatientRapportDiabete.Service
{
    public class NoteService
    {
        private readonly HttpClient _httpClient;
        public NoteService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7202");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
        }
        public async Task<List<NoteModel>?> Get(int patientId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

            try
            {
                // Effectuer la requête GET
                HttpResponseMessage response = await _httpClient.GetAsync($"gateway/notes?patientId={patientId}");

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
    }
}
