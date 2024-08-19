using System.ComponentModel.DataAnnotations;

namespace MediLaboSolutions.Models.InputModels
{
    public class PatientInputModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Le champs Nom est requis")]
        [StringLength(50)]
        [Display(Name = "Nom")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Le champs Prénom est requis")]
        [StringLength(50)]
        [Display(Name = "Prénom")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "La Date de Naissance est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de Naissance")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Le champs Genre est requis")]
        [StringLength(10)]
        [Display(Name = "Genre")]
        public string? Gender { get; set; }
        [StringLength(100)]
        [Display(Name = "Adresse Postale")]
        public string Address { get; set; }
        [Phone]
        [Display(Name = "Numéro de Téléphone")]
        public string PhoneNumber { get; set; }
    }
}
