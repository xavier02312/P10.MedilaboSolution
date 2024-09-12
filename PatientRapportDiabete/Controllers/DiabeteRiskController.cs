using Microsoft.AspNetCore.Mvc;
using PatientRapportDiabete.Models;
using PatientRapportDiabete.Service;

namespace PatientRapportDiabete.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiabeteRiskController : ControllerBase
    {
        private readonly DiabeteRiskService _diabeteRiskService;
        public DiabeteRiskController(DiabeteRiskService diabeteRiskService)
        {
            _diabeteRiskService = diabeteRiskService;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get([FromHeader] string authorization, [FromQuery] int id)
        {
            RiskEnum? risk = await _diabeteRiskService.GetRisk(id, authorization);  

            if(risk is null)
            {
                return NotFound("Patient not found");
            }
            return Ok(risk);
        }
    }
}
