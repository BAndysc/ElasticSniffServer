using System.Diagnostics;
using Server.Database;
using Server.Database.Models;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseRepository databaseRepository;

        public UserService(IDatabaseRepository databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }
    
        public Task<bool> AddAdminUser(string username, string password) => AddUser(username, password, true);

        public Task<bool> AddUser(string username, string password) => AddUser(username, password, false);
    
        public async Task<bool> VerifyUser(string username, string password)
        {
            var user = await databaseRepository.GetUser(username);
            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.KeyHash);
        }

        public Task<UserModel?> GetUser(string username)
        {
            return databaseRepository.GetUser(username);
        }

        public async Task<bool> IsAdmin(string username)
        {
            var user = await databaseRepository.GetUser(username);
            return user?.IsAdmin ?? false;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await databaseRepository.GetUser(username);
            return user != null;
        }

        private async Task<bool> AddUser(string username, string password, bool admin)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            Debug.Assert(hash != null);
            try
            {
                await databaseRepository.AddUser(username, hash, admin);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}