using PatientService.Domain;

namespace PatientService.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> CreateAsync(Patient patient);
        Task<Patient?> DeleteAsync(int id);
        Task<Patient> GetByIdAsync(int id);
        Task<List<Patient>> ListAsync();
        Task<Patient?> UpdateAsync(Patient patient);
    }
}
