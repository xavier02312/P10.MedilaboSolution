using Microsoft.EntityFrameworkCore;
using PatientMicroservice.Data;
using PatientMicroservice.Domain;

namespace MediLaboSolutions.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly LocalDbContext _context;

        public PatientRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<Patient?> DeleteAsync(int id)
        {
            var Patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (Patient != null)
            {
                _context.Patients.Remove(Patient);
                await _context.SaveChangesAsync();
            }
            return Patient;
        }

        public Patient? Get(int id) => _context.Patients.FirstOrDefault(i => i.Id == id);

        public async Task<Patient> GetByIdAsync(int id) => await _context.Patients.FindAsync(id);

        public async Task<IEnumerable<Patient>> ListAsync() => await Task.Run(() => _context.Patients.ToListAsync());

        public async Task<Patient?> UpdateAsync(Patient patient)
        {
            var patientModifie = await _context.Patients.FirstOrDefaultAsync(c => c.Id == patient.Id);

            if (patientModifie is not null)
            {
                patientModifie.FirstName = patient.FirstName;
                patientModifie.LastName = patient.LastName;
                patientModifie.PhoneNumber = patient.PhoneNumber;
                patientModifie.Address = patient.Address;
                patientModifie.DateOfBirth = patient.DateOfBirth;
                patientModifie.Gender = patient.Gender;

                await _context.SaveChangesAsync();
            }
            return patientModifie;
        }
    }
}
