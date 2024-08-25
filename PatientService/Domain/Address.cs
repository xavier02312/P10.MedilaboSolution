namespace PatientService.Domain
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
