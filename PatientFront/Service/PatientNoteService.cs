using PatientNote.Models.InputModels;
using PatientNote.Models.OutputModels;
using Serilog;
using System.Net;
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
        public async Task<(NoteOutputModel Note, HttpStatusCode HttpStatusCode)> Create(NoteInputModel noteModel)
        {
            try
            {
                // Effectuer la requête POST
                var response = await _httpClient.PostAsJsonAsync("/Note/Create", noteModel);

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Log.Error($"Unauthorized or Forbidden access while creating patient note. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                var patientNote = await response.Content.ReadFromJsonAsync<NoteOutputModel>();
                return (patientNote, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating patient note");
                return (null, HttpStatusCode.InternalServerError);
            }
        }
        // Liste Patient Notes
        public async Task<(List<NoteOutputModel> Notes, HttpStatusCode HttpStatusCode)> GetNotes(int patientId)
        {
            try
            {
                // Effectuer la requête GET
                var response = await _httpClient.GetAsync($"/Note/GetNotes?patientId={patientId}");

                if(response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Log.Error($"Unauthorized or Forbidden access while fetching patient notes. StatusCode: {response.StatusCode}");
                    return (null, response.StatusCode);
                }
                response.EnsureSuccessStatusCode();

                var notes = await response.Content.ReadFromJsonAsync<List<NoteOutputModel>>();
                return (notes, response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching patient notes");
                return (null, HttpStatusCode.InternalServerError);
            }
        }
    }
}
