using Microsoft.AspNetCore.Mvc;
using PatientService.Models.InputModels;
using PatientService.Service;
using Serilog;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        // Service de gestion des opérations CRUD
        private readonly IPatientServices _patientService;

        public PatientController(IPatientServices patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _patientService.ListAsync());
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }

        [HttpPost]
        [Route("Ajout")]
        public async Task<IActionResult> CreateAsync([FromBody] PatientInputModel input)
        {
            try
            {
                return Ok(await _patientService.CreateAsync(input));
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var patient = await _patientService.GetByIdAsync(id);

                if (patient is not null)
                {
                    return Ok(patient);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id, [FromBody] PatientInputModel input)
        {
            try
            {
                var patient = await _patientService.UpdateAsync(id, input);

                if (patient is not null)
                {
                    return Ok(patient);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] int id)
        {
            try
            {
                var patient = await _patientService.DeleteAsync(id);

                if (patient is not null)
                {
                    return Ok(patient);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }
    }
}
