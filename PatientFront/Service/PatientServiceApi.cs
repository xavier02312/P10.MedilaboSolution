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
                var patient = await _httpClient.GetFromJsonAsync<PatientOutputModel>($"/Patient/Get?id={id}");
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
                var response = await _httpClient.PostAsJsonAsync("/Patient/Ajout", input);
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
                var response = await _httpClient.PutAsJsonAsync($"/Patient/update?id={id}", input);
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
                var response = await _httpClient.DeleteAsync($"/Patient/delete?id={id}");
                response.EnsureSuccessStatusCode();

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
