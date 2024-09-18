﻿using Microsoft.AspNetCore.Mvc;
using PatientFront.Service;
using PatientNote.Models.InputModels;
using PatientService.Domain;
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
        public async Task<IActionResult> Index(int patientId)/* ... */
        {
            try
            {
                var patientNotes = await _patientNoteService.GetNotes(patientId);

                if (patientNotes == null)
                {
                    return NotFound();
                }
                return View(patientNotes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching patient notes");
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: 
        // Action pour afficher le formulaire de création d'une note à un patient
        [HttpGet]
        public IActionResult Ajout(int patientId)
        {
            var noteModel = new NoteInputModel
            {
                PatientId = patientId
            };
            return View(noteModel);
        }
        [HttpPost]
        public async Task<IActionResult> Ajout([Bind("PatientId","Content")] NoteInputModel noteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _patientNoteService.Create(noteModel);
                    return RedirectToAction("Index", "PatientsNotes");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating patient note");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
