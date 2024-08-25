using Microsoft.AspNetCore.Mvc;
using PatientService.Models;
using PatientService.Service;
using Serilog;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var token = await _authenticationServices.Login(loginModel.Username, loginModel.Password);
                if (token != "")
                {
                    return Ok(new { value = token });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
            }
            return NotFound();
        }
    }
}
