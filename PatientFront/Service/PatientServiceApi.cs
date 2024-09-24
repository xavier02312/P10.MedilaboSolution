using PatientService.Models.InputModels;
using PatientService.Models.OutputModels;
using Serilog;
using System.Net;
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
        public async Task<(List<PatientOutputModel> Patients, HttpStatusCode HttpStatusCode)> ListAsync()
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync("/Patient/list");

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Log.Error($"Unauthorized or Forbidden access while fetching patient list. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                var patients = await response.Content.ReadFromJsonAsync<List<PatientOutputModel>>();
                return (patients, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching patient list");
                return (new List<PatientOutputModel>(), HttpStatusCode.InternalServerError);
            }
        }
        // Detail un Patient
        public async Task<(PatientOutputModel Patients, HttpStatusCode HttpStatusCode)> GetByIdAsync(int id)
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync($"/Patient/Get?id={id}");

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Log 
                    Log.Error($"Unauthorized or Forbidden access while fetching patient details. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return (patient, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error fetching patient with ID {id}");
                return (null, HttpStatusCode.InternalServerError);
            }
        }
        // Créér un Patient
        public async Task<(PatientOutputModel Patients, HttpStatusCode HttpStatusCode)> CreateAsync(PatientInputModel input)
        {
            try
            {
                // Effectuer la requête POST
                var response = await _httpClient.PostAsJsonAsync("/Patient/Ajout", input);

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Log l'erreur et retourner null
                    Log.Error($"Unauthorized or Forbidden access while creating patient. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return (patient, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating patient");
                return (null, HttpStatusCode.InternalServerError);
            }
        }
        // Modifier un Patient
        public async Task<(PatientOutputModel Patients, HttpStatusCode HttpStatusCode)> UpdatePatientAsync(int id, PatientInputModel input) 
        {
            try
            {
                // Effectuer la requête PUT
                var response = await _httpClient.PutAsJsonAsync($"/Patient/update?id={id}", input);

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Log l'erreur et retourner null
                    Log.Error($"Unauthorized or Forbidden access while updating patient. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return (patient, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error updating patient with ID {id}");
                return (null, HttpStatusCode.InternalServerError);
            }
        }
        // Supprimer un Patient
        public async Task<(List<PatientOutputModel> Patients, HttpStatusCode HttpStatusCode)> DeletePatientAsync(int id)
        {
            try
            {
                // Effectuer la requête POST
                var response = await _httpClient.DeleteAsync($"/Patient/delete?id={id}");

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Log l'erreur 
                    Log.Error($"Unauthorized or Forbidden access while deleting patient. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                // Effectuer la requête GET
                var patients = await _httpClient.GetFromJsonAsync<List<PatientOutputModel>>("/Patient/list");
                return (patients, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error deleting patient with ID {id}");
                return (new List<PatientOutputModel>(), HttpStatusCode.InternalServerError);
            }
        }
    }
}
