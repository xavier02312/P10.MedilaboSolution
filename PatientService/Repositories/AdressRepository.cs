using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Domain;

namespace PatientService.Repositories
{
    public class AdressRepository : IAdressRepository
    {
        private readonly LocalDbContext _context;
        public AdressRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Create(Address address)
        {
            try
            {
                var existingAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.Name == address.Name);
                if (existingAddress is null)
                {
                    await _context.Addresses.AddAsync(address);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    address = existingAddress;
                }
                return address;
            }
            catch
            {
                throw;
            }
        }
        public async Task<Address?> Delete(int id)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(address => address.Id == id);
                if (address is not null)
                {
                    _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();
                }
                return address;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Address?> Read(int id)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(address => address.Id == id);
                return address;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Address?> Read(string name)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(address => address.Name == name);
                return address;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Address?> Update(Address address)
        {
            try
            {
                var addressToUpdate = await _context.Addresses.FirstOrDefaultAsync(p => p.Id == address.Id);
                if (addressToUpdate is not null)
                {
                    addressToUpdate.Name = address.Name;
                    await _context.SaveChangesAsync();
                }
                return addressToUpdate;
            }
            catch
            {
                throw;
            }
        }
    }
}
