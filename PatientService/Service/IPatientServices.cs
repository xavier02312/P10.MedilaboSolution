using PatientService.Models.InputModels;
using PatientService.Models.OutputModels;

namespace PatientService.Service
{
    public interface IPatientServices
    {
        Task<List<PatientOutputModel>> ListAsync(); // Liste Patient
        Task<PatientOutputModel> CreateAsync(PatientInputModel input); // Creér Patient
        Task<PatientOutputModel> GetByIdAsync(int id); // Récupérer un Patient
        public PatientOutputModel? Get(int id); // Si le patient n'existe pas 
        Task<PatientOutputModel?> UpdateAsync(int id, PatientInputModel input); // Modifier un patient
        Task<PatientOutputModel?> DeleteAsync(int id); // Supprimer un Patient
    }
}
