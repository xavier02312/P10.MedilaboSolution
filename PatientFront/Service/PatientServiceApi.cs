using PatientService.Models.InputModels;
using PatientService.Models.OutputModels;

namespace PatientFront.Service
{
    public class PatientServiceApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PatientServiceApi> _logger;

        public PatientServiceApi(HttpClient httpClient, ILogger<PatientServiceApi> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        // Liste Patient
        public async Task<List<PatientOutputModel>> ListAsync()
        {
            var patients = await _httpClient.GetFromJsonAsync<List<PatientOutputModel>>("/Patient/list");

            return patients;
        }
        // Detail un Patient
        public async Task<PatientOutputModel> GetByIdAsync(int id)
        {
            var patient = await _httpClient.GetFromJsonAsync<PatientOutputModel>($"/Patient/Get?id={id}");

            return patient;
        }
        // Créér un Patient
        public async Task<PatientOutputModel> CreateAsync(PatientInputModel input)
        {
            var response = await _httpClient.PostAsJsonAsync("/Patient/Ajout", input);
            response.EnsureSuccessStatusCode();

            var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
            return patient;
        }
        // Modifier un Patient
        public async Task<PatientOutputModel> UpdatePatientAsync(int id, PatientInputModel input) 
        {
            var response = await _httpClient.PutAsJsonAsync($"/Patient/update?id={id}", input);
            response.EnsureSuccessStatusCode();

            var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
            
            return patient;
        }
        // Supprimer un Patient
        public async Task<List<PatientOutputModel>> DeletePatientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/Patient/delete?id={id}");
            response.EnsureSuccessStatusCode();

            var patients = await _httpClient.GetFromJsonAsync<List<PatientOutputModel>>("/Patient/list");
            return patients;
        }
    }
}
