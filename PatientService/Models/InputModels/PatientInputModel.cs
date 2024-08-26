﻿using System.ComponentModel.DataAnnotations;

namespace PatientService.Models.InputModels
{
    public class PatientInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
