using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientRapportDiabete.Models;
using Serilog;

namespace PatientFront.Controllers
{
    public class PatientsRapportDiabeteController : Controller
    {
        private readonly PatientRapportDiabeteService _patientRapportDiabeteService;
        public PatientsRapportDiabeteController(PatientRapportDiabeteService patientRapportDiabeteService)
        {
            _patientRapportDiabeteService = patientRapportDiabeteService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int patientId)
        {
            try
            {
                var patient = await _patientRapportDiabeteService.Get(patientId); // Récupère le patient

                if (patient == null)
                {
                    // View Error
                    return View("404");
                }

                var riskLevel = await _patientRapportDiabeteService.GetRiskLevel(patient.Id); // Calcul le risque

                Log.Information($"Patient ID: {patient.Id}, Risk Level: {riskLevel}");

                var patientRiskLevels = new Dictionary<string, RiskEnum?>
                {
                    { patient.LastName, riskLevel },
                };

                ViewBag.PatientRiskLevels = patientRiskLevels;

                return View(new List<PatientModel> { patient });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
        // Affiche le risque
        [HttpGet]
        public async Task<IActionResult> GetRisk(int patientId)
        {
            try
            {
                var riskLevel = await _patientRapportDiabeteService.GetRiskLevel(patientId);

                if (riskLevel == null)
                {
                    // View Error
                    return View("404");
                }

                ViewBag.RiskLevel = riskLevel;

                return View("RiskLevel", riskLevel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
