using Microsoft.EntityFrameworkCore;
using NaviLib.MyTypes;


namespace RgesNaviApi.DataBaseContext
{
    public class ApplicationContext : DbContext
    {

        public DbSet<EnergyObject> EnergyObjects { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=rgesobjects;Trusted_Connection=True;Encrypt=False;");
        }
    }
}
