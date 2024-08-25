using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Domain;

namespace PatientService.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly LocalDbContext _context;

        public PatientRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient?> DeleteAsync(int id)
        {
            var Patient = await _context.Patients.Include(p => p.Address).FirstOrDefaultAsync(p => p.Id == id);

            if (Patient != null)
            {
                _context.Patients.Remove(Patient);
                await _context.SaveChangesAsync();
            }
            return Patient;
        }

        public async Task<Patient> GetByIdAsync(int id) => await _context.Patients.FindAsync(id);

        public async Task<List<Patient>> ListAsync() => await Task.Run(() => _context.Patients.Include(p => p.Address).ToListAsync());

        public async Task<Patient?> UpdateAsync(Patient patient)
        {
            var patientModifie = await _context.Patients.Include(p => p.Address).FirstOrDefaultAsync(c => c.Id == patient.Id);

            if (patientModifie is not null)
            {
                patientModifie.FirstName = patient.FirstName;
                patientModifie.LastName = patient.LastName;
                patientModifie.DateOfBirth = patient.DateOfBirth;
                patientModifie.Gender = patient.Gender;
                if (patientModifie.Address is not null)
                {
                    patientModifie.Address = patient.Address;
                }
                if (patientModifie.PhoneNumber is not null)
                {
                    patientModifie.PhoneNumber = patient.PhoneNumber;
                }
                await _context.SaveChangesAsync();
            }
            return patientModifie;
        }
    }
}
