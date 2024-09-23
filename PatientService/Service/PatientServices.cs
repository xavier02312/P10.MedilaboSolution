using PatientService.Domain;
using PatientService.Models.InputModels;
using PatientService.Models.OutputModels;
using PatientService.Repositories;

namespace PatientService.Service
{
    public class PatientServices : IPatientServices
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAdressRepository _adressRepository;

        public PatientServices(IPatientRepository patientRepository, IAdressRepository adressRepository)
        {
            _patientRepository = patientRepository;
            _adressRepository = adressRepository;
        }

        public async Task<PatientOutputModel> CreateAsync(PatientInputModel input)
        {
            try
            {
                var patients = await _patientRepository.CreateAsync(await Entity(input, 0));
                return ToOutputModel(patients);
            }
            catch 
            {
                throw;
            }
        }
 
        public async Task<List<PatientOutputModel>> ListAsync()
        {
            try
            {
                var list = new List<PatientOutputModel>();
                var Patients = await _patientRepository.ListAsync();

                foreach (var Patient in Patients)
                {
                    list.Add(ToOutputModel(Patient));
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PatientOutputModel> GetByIdAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(id);

                if (patient is not null)
                {
                    var output = ToOutputModel(patient);

                    // Récupérez l'adresse à partir de l'ID de l'adresse
                    if (patient.AddressId.HasValue)
                    {
                        var address = await _adressRepository.Read(patient.AddressId.Value);
                        if (address != null)
                        {
                            output.Address = address.Name;
                        }
                    }

                    return output;
                }
                    return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PatientOutputModel?> UpdateAsync(int id, PatientInputModel input)
        {
            try
            {
                var patientUpdated = await _patientRepository.UpdateAsync(await Entity(input, id));

                if (patientUpdated is not null)
                {
                    return ToOutputModel(patientUpdated);
                }
                return null;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<PatientOutputModel?> DeleteAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.DeleteAsync(id);

                if (patient is null)
                {
                    return null;
                }
                return ToOutputModel(patient);
            }
            catch
            {
                throw;
            }
        }

        private async Task<Patient> Entity(PatientInputModel input, int id)
        {
            var patient = new Patient()
            {
                Id = id,
                FirstName = input.FirstName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                Gender = input.Gender,
            };

            if (input.Address is not null)
            {
                var address = await _adressRepository.Create(new Address { Name = input.Address });
                patient.Address = address;
                patient.AddressId = address.Id;
            }

            if (input.PhoneNumber is not null)
            {
                patient.PhoneNumber = input.PhoneNumber;
            }
            return patient;
        }
        private PatientOutputModel ToOutputModel(Patient patient)
        {
            var output = new PatientOutputModel()
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
            };

            if (patient.Address is not null)
            {
                output.Address = patient.Address.Name;
            }
            if (patient.PhoneNumber is not null)
            {
                output.PhoneNumber = patient.PhoneNumber;
            }
            return output;
        }
    }
}
