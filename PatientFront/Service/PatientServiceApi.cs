using PatientService.Models.InputModels;
using PatientService.Models.OutputModels;
using Serilog;
using System.Net.Http.Headers;

namespace PatientFront.Service
{
    public class PatientServiceApi
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientServiceApi> _logger;

        public PatientServiceApi(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientServiceApi> logger)
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
        // Liste Patient
        public async Task<List<PatientOutputModel>> ListAsync()
        {
            try
            {
                // Effectuer la requête GET
                var patients = await _httpClient.GetFromJsonAsync<List<PatientOutputModel>>("/Patient/list");
                return patients;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching patient list");
                return new List<PatientOutputModel>();
            }
        }
        // Detail un Patient
        public async Task<PatientOutputModel> GetByIdAsync(int id)
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync($"/Patient/Get?id={id}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Log l'erreur et retourner null
                    Log.Error($"Unauthorized or Forbidden access while fetching patient details. StatusCode: {response.StatusCode}");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error fetching patient with ID {id}");
                return null;
            }
        }
        // Créér un Patient
        public async Task<PatientOutputModel> CreateAsync(PatientInputModel input)
        {
            try
            {
                // Effectuer la requête POST
                var response = await _httpClient.PostAsJsonAsync("/Patient/Ajout", input);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Log l'erreur et retourner null
                    Log.Error($"Unauthorized or Forbidden access while creating patient. StatusCode: {response.StatusCode}");
                    return null;
                }
                response.EnsureSuccessStatusCode();

                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating patient");
                return null;
            }
        }
        // Modifier un Patient
        public async Task<PatientOutputModel> UpdatePatientAsync(int id, PatientInputModel input) 

        {
            try
            {
                // Effectuer la requête PUT
                var response = await _httpClient.PutAsJsonAsync($"/Patient/update?id={id}", input);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Log l'erreur et retourner null
                    Log.Error($"Unauthorized or Forbidden access while updating patient. StatusCode: {response.StatusCode}");
                    return null;
                }
                response.EnsureSuccessStatusCode();

                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error updating patient with ID {id}");
                return null;
            }
        }
        // Supprimer un Patient
        public async Task<List<PatientOutputModel>> DeletePatientAsync(int id)
        {
            try
            {
                // Effectuer la requête POST
                var response = await _httpClient.DeleteAsync($"/Patient/delete?id={id}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Log l'erreur 
                    Log.Error($"Unauthorized or Forbidden access while deleting patient. StatusCode: {response.StatusCode}");
                    return null;
                }
                response.EnsureSuccessStatusCode();

                // Effectuer la requête GET
                var patients = await _httpClient.GetFromJsonAsync<List<PatientOutputModel>>("/Patient/list");
                return patients;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error deleting patient with ID {id}");
                return new List<PatientOutputModel>();
            }
        }
    }
}
