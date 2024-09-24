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
                var (Patients, HttpStatusCode) = await _patientServiceApi.ListAsync();

                if (Patients == null)
                {
                    // Page Error
                    return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                }
                return View(Patients);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "Error fetching patient list");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var (Patients, HttpStatusCode) = await _patientServiceApi.GetByIdAsync(id);

                if (Patients == null)
                {
                    // Page Error
                    return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                }
                return View(Patients);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error fetching patient with ID {id}");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
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
                    var (Patients, HttpStatusCode) = await _patientServiceApi.CreateAsync(input);

                    if (Patients == null)
                    {
                        // Page Error
                        return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                    }

                    // Message de confirmation à TempData
                    TempData["SuccessMessage"] = "Le patient a été ajouté avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                // Error model no valid
                ModelState.AddModelError(string.Empty, "Le modèle n'est pas valide. Veuillez vérifier les informations saisies.");
                
                return View(nameof(Ajout));
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

                var (Patients, HttpStatusCode) = await _patientServiceApi.GetByIdAsync(id);

                if (Patients == null)
                {
                    // Page Error
                    return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                }

                var inputModel = new PatientInputModel
                {
                    FirstName = Patients.FirstName,
                    LastName = Patients.LastName,
                    DateOfBirth = Patients.DateOfBirth,
                    Gender = Patients.Gender,
                    Address = Patients.Address,
                    PhoneNumber = Patients.PhoneNumber
                };

                return View(inputModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error fetching patient with ID {id}");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PatientInputModel input) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (Patients, HttpStatusCode) = await _patientServiceApi.UpdatePatientAsync(id, input);

                    if (Patients == null)
                    {
                        // Page Error
                        return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                    }

                    // Message de confirmation à TempData
                    TempData["SuccessMessage"] = "Le patient a été modifier avec succès.";

                    return RedirectToAction(nameof(Index), new { id = Patients.Id });
                }
                return View(input);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error updating patient with ID {id}");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
        // GET: 
        // Action pour afficher la comfirmation 
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var (Patients, HttpStatusCode) = await _patientServiceApi.GetByIdAsync(id);

            if (Patients == null)
            {
                // Page Error
                return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
            }
            
            return View(Patients);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (Patients, HttpStatusCode) = await _patientServiceApi.DeletePatientAsync(id);

                if (Patients == null)
                {
                    return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                }

                // Message de confirmation de suppression
                TempData["SuccessMessage"] = "Le patient a été supprimé avec succès.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, $"Error deleting patient with ID {id}");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
    }
}
