using PatientService.Domain;

namespace PatientService.Repositories
{
    public interface IAdressRepository
    {
        Task<Address> Create(Address address);
        Task<Address?> Read(int id);
        Task<Address?> Read(string name);
        Task<Address?> Update(Address address);
        Task<Address?> Delete(int id);
    }
}
