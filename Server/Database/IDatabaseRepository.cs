using Server.Database.Models;

namespace Server.Database
{
    public interface IDatabaseRepository
    {
        public Task AddUser(string name, string hash, bool admin);
        public Task<UserModel?> GetUser(string name);
        Task Log(HttpRequest request, string text);
        Task Log(string? ip, string method, string userAgent, string text, DateTime? date = null);
    }
}