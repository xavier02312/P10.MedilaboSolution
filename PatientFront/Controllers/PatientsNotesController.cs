using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientNote.Models.InputModels;
using Serilog;

namespace PatientFront.Controllers
{
    public class PatientsNotesController : Controller
    {
        private readonly PatientNoteService _patientNoteService;
        public PatientsNotesController(PatientNoteService patientNoteService)
        {
            _patientNoteService = patientNoteService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int patientId)
        {
            try
            {
                var (Notes, HttpStatusCode) = await _patientNoteService.GetNotes(patientId);

                if (Notes == null)
                {
                    // View Error
                    return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                }
                return View(Notes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching patient notes");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
        // GET: 
        // Action pour afficher le formulaire de création d'une note à un patient
        [HttpGet]
        public IActionResult Ajout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Ajout([Bind("PatientId","Content")] NoteInputModel noteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (Note, HttpStatusCode) = await _patientNoteService.Create(noteModel);

                    if (Note == null)
                    {
                        return RedirectToAction("Error", "Home", new { statusCode = (int)HttpStatusCode });
                    }
                    return RedirectToAction("Index", "Patients");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating patient note");
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
    }
}
