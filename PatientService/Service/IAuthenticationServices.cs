namespace PatientService.Service
{
    public interface IAuthenticationServices
    {
        Task<string> Login(string username, string password);
    }
}
