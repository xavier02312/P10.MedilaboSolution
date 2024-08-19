using System.ComponentModel.DataAnnotations;

namespace PatientMicroservice.Domain
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Nom")]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Prénom")]
        public string? LastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date de Naissance")]
        public DateTime DateOfBirth { get; set; }
        [Required,StringLength(10)]
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
