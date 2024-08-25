using Microsoft.AspNetCore.Identity;

namespace PatientService.Data
{
    public class DatabaseGestionRoles
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseGestionRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<bool> EnsureOrganizerIsCreated()
        {
            if (await _userManager.FindByNameAsync("organizer") is not null)
            {
                return true;
            }
            var user = new IdentityUser()
            {
                UserName = "organizer"
            };
            if (!await _roleManager.RoleExistsAsync("organizer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("organizer"));
            }
            var result = await _userManager.CreateAsync(user, "6yb64nOav4M?JmHzn");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "organizer");
                return true;
            }
            return false;
        }
        public async Task<bool> EnsurePractitionerIsCreated()
        {
            if (await _userManager.FindByNameAsync("practitioner") is not null)
            {
                return true;
            }
            var user = new IdentityUser()
            {
                UserName = "practitioner"
            };
            if (!await _roleManager.RoleExistsAsync("practitioner"))
            {
                await _roleManager.CreateAsync(new IdentityRole("practitioner"));
            }
            var result = await _userManager.CreateAsync(user, "6yb64nOav4M?JmHzn");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "practitioner");
                return true;
            }
            return false;
        }

        public async Task<bool> EnsureAdminIsCreated()
        {
            if (await _userManager.FindByNameAsync("admin") is not null)
            {
                return true;
            }
            var user = new IdentityUser()
            {
                UserName = "admin"
            };
            if (!await _roleManager.RoleExistsAsync("practitioner"))
            {
                await _roleManager.CreateAsync(new IdentityRole("practitioner"));
            }
            if (!await _roleManager.RoleExistsAsync("organizer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("organizer"));
            }
            var result = await _userManager.CreateAsync(user, "6yb64nOav4M?JmHzn");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "practitioner");
                await _userManager.AddToRoleAsync(user, "organizer");
                return true;
            }
            return false;
        }
    }
}
