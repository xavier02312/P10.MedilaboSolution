using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientService.Models.InputModels;
using Serilog;

namespace PatientFront.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PatientServiceApi _patientServiceApi;

        public PatientsController(PatientServiceApi patientServiceApi)
        {
            _patientServiceApi = patientServiceApi;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var patients = await _patientServiceApi.ListAsync();
                if (patients == null)
                {
                    return NotFound();
                }
                return View(patients);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "Error fetching patient list");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var patient = await _patientServiceApi.GetByIdAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error fetching patient with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: 
        // Action pour afficher le formulaire de création de patient
        [HttpGet]
        public IActionResult Ajout()
        {
            return  View();
        }
        [HttpPost]
        public async Task<IActionResult> Ajout([Bind("FirstName, LastName, DateOfBirth, Gender, Address, PhoneNumber")]
        PatientInputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _patientServiceApi.CreateAsync(input);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Ajout));
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "Error creating patient");
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: Patient/Update?id={id}
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                if (id == 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                var patient = await _patientServiceApi.GetByIdAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }

                var inputModel = new PatientInputModel
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    Address = patient.Address,
                    PhoneNumber = patient.PhoneNumber
                };

                return View(inputModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error fetching patient with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PatientInputModel input) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var patient = await _patientServiceApi.UpdatePatientAsync(id, input);
                    return RedirectToAction(nameof(Index), new { id = patient.Id });
                }
                return View(input);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error updating patient with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patients = await _patientServiceApi.DeletePatientAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error deleting patient with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
