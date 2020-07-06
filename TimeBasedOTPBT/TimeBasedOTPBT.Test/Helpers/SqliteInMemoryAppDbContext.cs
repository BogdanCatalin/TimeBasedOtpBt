using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TimeBasedOTPBT.Persistence.Contexts;

namespace TimeBasedOTPBT.Test.Helpers
{
    public class SqliteInMemoryAppDbContext : SqliteDataContext
    {
        public SqliteInMemoryAppDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            options.UseSqlite(connection);
        }
    }
}
