using PatientMicroservice.Domain;

namespace MediLaboSolutions.Repositories
{
    public interface IPatientRepository
    {
        Task CreateAsync(Patient patient);
        Task<Patient?> DeleteAsync(int id);
        public Patient? Get(int id);
        Task<Patient> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> ListAsync();
        Task<Patient?> UpdateAsync(Patient patient);
    }
}
