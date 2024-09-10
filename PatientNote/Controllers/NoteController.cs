using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientNote.Models.InputModels;
using PatientNote.Service;
using Serilog;

namespace PatientNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("practitioner")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] NoteInputModel inputModel)
        {
            try
            {
                return Ok(await _noteService.Create(inputModel));
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }

        [HttpGet]
        [Route("GetNotes")]
        public async Task<IActionResult> GetNotes([FromQuery] int patientId)
        {
            try
            {
                return Ok(await _noteService.GetNotes(patientId));
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem();
            }
        }
    }
}
