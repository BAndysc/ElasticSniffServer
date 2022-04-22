using Microsoft.EntityFrameworkCore;
using Server.Database.Models;

namespace Server.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions) {}

        public DbSet<UserModel> Users => Set<UserModel>();
        
        public DbSet<LogModel> Logs => Set<LogModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
        }
    }
}