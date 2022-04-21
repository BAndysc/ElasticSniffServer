namespace Server.Services
{
    public interface IUserService
    {
        Task<bool> AddAdminUser(string username, string password);
        Task<bool> AddUser(string username, string password);
        Task<bool> VerifyUser(string username, string password);
        Task<bool> IsAdmin(string username);
        Task<bool> UserExists(string username);
    }
}