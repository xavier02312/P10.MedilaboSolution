using MediLaboSolutions.Models.InputModels;
using MediLaboSolutions.Models.OutputModels;
using MediLaboSolutions.Repositories;
using MediLaboSolutions.Service;
using PatientMicroservice.Domain;

namespace PatientMicroservice.Service
{
    public class PatientServices : IPatientServices
    {
        private readonly IPatientRepository _repository;

        public PatientServices(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PatientOutputModel>> ListAsync()
        {
            var list = new List<PatientOutputModel>();
            var Patients = await _repository.ListAsync();

            foreach (var Patient in Patients)
            {
                list.Add(ToOutputModel(Patient));
            }
            return list;
        }

        public async Task<PatientOutputModel> CreateAsync(PatientInputModel input)
        {
            var patient = new Patient
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                Gender = input.Gender,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
            };
            await _repository.CreateAsync(patient);
            return ToOutputModel(patient);
        }

        public async Task<PatientOutputModel> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient == null)
            {
                return null;
            }
            return ToOutputModel(patient);
        }

        public PatientOutputModel? Get(int id)
        {
            var patient = _repository.Get(id);

            if (patient is not null)
            {
                return ToOutputModel(patient);
            }
            return null;
        }

        public async Task<PatientOutputModel?> UpdateAsync(int id, PatientInputModel input)
        {
            var patient = await _repository.UpdateAsync(new Patient
            {
                Id = input.Id,
                FirstName = input.FirstName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                Gender = input.Gender,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
            });

            if (patient is not null)
            {
                return ToOutputModel(patient);
            }
            return null;
        }

        public async Task<PatientOutputModel?> DeleteAsync(int id)
        {
            var patient = await _repository.DeleteAsync(id);

            if (patient is not null)
            {
                return ToOutputModel(patient);
            }
            return null;
        }

        private PatientOutputModel ToOutputModel(Patient patient) => new PatientOutputModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
            };
    }
}
