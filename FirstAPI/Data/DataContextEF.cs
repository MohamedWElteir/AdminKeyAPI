using Microsoft.EntityFrameworkCore;
using FirstAPI.Models;
namespace FirstAPI.Data
{
    public class DataContextEf : DbContext
    {
        private readonly IConfiguration _config;
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }
        public virtual DbSet<UserSalary> UserSalary { get; set; }
        public DataContextEf(IConfiguration configuration)
        {
            _config = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           if (!optionsBuilder.IsConfigured)
           {
               optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
           }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            modelBuilder.Entity<User>().ToTable("Users", "TutorialAppSchema").HasKey(x => x.UserId);
            modelBuilder.Entity<UserJobInfo>().HasKey(x => x.UserId);
            modelBuilder.Entity<UserSalary>().HasKey(x => x.UserId); 
        }

    }

}