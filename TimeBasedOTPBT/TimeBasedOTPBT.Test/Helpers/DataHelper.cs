using TimeBasedOTPBT.Persistence.Contexts;
using TimeBasedOTPBT.Test.Helpers.Password;

namespace TimeBasedOTPBT.Test.Helpers
{
    public class DataHelper
    {
        public Factory Factory { get; set; }

        public SqliteDataContext CreateDbContext(IPasswordHelper passwordHelper)
        {
            var dbContext = new SqliteInMemoryAppDbContext(null);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Factory = new Factory(dbContext, passwordHelper);

            return dbContext;
        }
    }
}
