using Server.Database.Models;

namespace Server.Database
{
    public interface IDatabaseRepository
    {
        public Task AddUser(string name, string hash, bool admin);
        public Task<UserModel?> GetUser(string name);
    }
}