using Microsoft.EntityFrameworkCore;
using Server.Database.Models;

namespace Server.Database
{
    public class DatabaseRepository : IDatabaseRepository, System.IDisposable
    {
        private readonly DatabaseContext databaseContext;
        private readonly IServiceScope scope;

        public DatabaseRepository(IServiceProvider serviceProvider)
        {
            scope = serviceProvider.CreateScope();
            databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }
        
        public async Task Log(string? ip, string method, string userAgent, string text, DateTime? date = null)
        {
            await databaseContext.Logs.AddAsync( LogModel.Create(ip, method, text, userAgent ?? "", date));
            await databaseContext.SaveChangesAsync();
        }

        public async Task Log(HttpRequest request, string text)
        {
            await Log(request.HttpContext.Connection.RemoteIpAddress?.ToString(), request.Method,request.Headers.UserAgent, text);
        }
    
        public async Task AddUser(string name, string hash, bool admin)
        {
            await using var transaction = await databaseContext.Database.BeginTransactionAsync();
            bool exists = (await databaseContext.Users.FirstOrDefaultAsync(x => x.User == name)) != null;
            if (exists)
                throw new Exception("User " + name + " already exists! Aborting");
            var userModel = new UserModel()
            {
                User = name,
                KeyHash = hash,
                IsAdmin = admin,
            };
            await databaseContext.Users.AddAsync(userModel);
            await databaseContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<UserModel?> GetUser(string name)
        {
            return await databaseContext.Users.FirstOrDefaultAsync(u => u.User == name);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}