using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TimeBasedOTPBT.Persistence.Contexts
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Entities.User> Users { get; set; }
    }
}
