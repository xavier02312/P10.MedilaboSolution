using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatientService.Service
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthenticationServices(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<string> Login(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user is null)
                {
                    return "";
                }
                var result = await _userManager.CheckPasswordAsync(user, password);
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName!)
                };
                foreach (var role in roles)
                {
                    claims.Add(new(ClaimTypes.Role, role));
                }
                if (result)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]!);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(24),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
            }
            catch
            {
                throw;
            }
            return "";
        }
    }
}
