using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Server.Database.Models;
using Server.Database.Models.Index;

namespace Server.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseLoggerFactory(new NullLoggerFactory());
        }

        public DbSet<UserModel> Users => Set<UserModel>();
        
        public DbSet<LogModel> Logs => Set<LogModel>();

        public DbSet<SniffModel> SniffsIndex => Set<SniffModel>();

        public DbSet<SniffNumberFieldModel> SniffNumbers => Set<SniffNumberFieldModel>();

        public DbSet<SniffTextFieldModel> SniffTexts => Set<SniffTextFieldModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SniffNumberFieldModel>().HasKey(x => new { x.SniffModelId, x.Field, x.Value });
            modelBuilder.Entity<SniffTextFieldModel>().HasKey(x => new { x.SniffModelId, x.Field, x.Id });
            
        }
    }
}