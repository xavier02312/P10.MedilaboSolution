using PatientNote.Models.InputModels;
using PatientNote.Models.OutputModels;
using Serilog;
using System.Net.Http.Headers;

namespace PatientFront.Service
{
    public class PatientNoteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientNoteService> _logger;
        public PatientNoteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientNoteService> logger)
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
        // Créer une Note à un Patient
        public async Task<NoteOutputModel> Create(NoteInputModel noteModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/Note/Create", noteModel);
                response.EnsureSuccessStatusCode();
                var patientNote = await response.Content.ReadFromJsonAsync<NoteOutputModel>();
                return patientNote;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating patient note");
                throw;
            }
        }
        // Liste Patient Notes
        public async Task<List<NoteOutputModel>> GetNotes(int patientId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/Note/GetNotes?patientId={patientId}");

                var notes = await response.Content.ReadFromJsonAsync<List<NoteOutputModel>>();
                return notes;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching patient notes");
                return null;
            }
        }
    }
}
