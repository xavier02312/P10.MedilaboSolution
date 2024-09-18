namespace PatientService.Domain
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? PhoneNumber { get; set; }

        // Permet d'avoir le format "yyyy/MM/dd 
        public string GetFormattedDateOfBirth()
        {
            return DateOfBirth.ToString("yyyy/MM/dd");
        }
    }
}
