using Microsoft.AspNetCore.Mvc;
using PatientService.Models.InputModels;
using PatientService.Service;

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

        /// <summary>
        /// GET: PatientController
        /// </summary>
        /// <returns>200</returns>
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var patients = await _patientService.ListAsync();
                if (patients == null || !patients.Any())
                {
                    return NotFound("Aucun patient trouvé");
                }
                return Ok(patients);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur interne s'est produite");
            }
        }

        /// <summary>
        /// POST: PatientController
        /// </summary>
        /// <param name="input"></param>
        /// <returns>201</returns>
        [HttpPost]
        [Route("Ajout")]
        public async Task<IActionResult> AddPatient([FromBody] PatientInputModel input)
        {
            try
            {
                var result = await _patientService.CreateAsync(input);
                return CreatedAtAction(nameof(GetPatientById), new { id = result.Id }, result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur interne s'est produite");
            }
        }

        /// <summary>
        /// GET: PatientController/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 ok</returns>
        /// <returns>404 Si pas trouver</returns>
        [HttpGet]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound("Patient introuvable");
            }
            return Ok(patient);
        }

        /// <summary>
        /// GET: PatientController/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns>200</returns>
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdatePatientId([FromQuery] int id, [FromBody] PatientInputModel input)
        {
            try
            {
                var patient = await _patientService.UpdateAsync(id, input);

                if (patient == null)
                {
                    return NotFound("Patient introuvable");
                }

                var updatedPatients = await _patientService.ListAsync();
                return Ok(updatedPatients);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur interne s'est produite");
            }
        }

        /// <summary>
        /// DELETE: PatientController/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200</returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeletePatientById([FromQuery] int id)
        {
            try
            {
                var patient = await _patientService.DeleteAsync(id);

                if (patient == null)
                {
                    return NotFound("Patient introuvable");
                }

                var updatedPatients = await _patientService.ListAsync();
                return Ok(updatedPatients);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur interne s'est produite");
            }

        }

        /*// GET: PatientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
