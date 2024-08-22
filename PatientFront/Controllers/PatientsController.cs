using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientService.Domain;
using PatientService.Models.InputModels;
using PatientService.Models.OutputModels;

namespace PatientFront.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PatientServiceApi _patientServiceApi;

        public PatientsController(PatientServiceApi patientServiceApi)
        {
            _patientServiceApi = patientServiceApi;
        }
        public async Task<IActionResult> Index()
        {
            var patients = await _patientServiceApi.ListAsync();

            if (patients == null)
            {
                return NotFound();
            }
            return View(patients);
        }
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientServiceApi.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }
        // GET: Voitures/Create
        // Action pour afficher le formulaire de création de patient
        public IActionResult Ajout()
        {
            return  View();
        }
        [HttpPost]
        public async Task<IActionResult> Ajout([Bind("FirstName, LastName, DateOfBirth, Gender, Address, PhoneNumber")]
        PatientInputModel input)
        {
            if (ModelState.IsValid)
            {
                await _patientServiceApi.CreateAsync(input);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Ajout));
        }
        // GET: Patient/Update?id={id}
        public async Task<IActionResult> Update(int id)
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
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PatientInputModel input) 
        {
            if (ModelState.IsValid)
            {
                var patient = await _patientServiceApi.UpdatePatientAsync(id, input);
                return RedirectToAction(nameof(Details), new { id = patient.Id });
            }

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var patients = await _patientServiceApi.DeletePatientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
