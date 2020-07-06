using System.Collections.Generic;
using TimeBasedOTPBT.Persistence.Contexts;
using TimeBasedOTPBT.Persistence.Entities;
using TimeBasedOTPBT.Test.Helpers.Password;

namespace TimeBasedOTPBT.Test.Helpers
{
    public class Factory
    {
        public SqliteDataContext Context { get; set; }
        
        private readonly IPasswordHelper _passwordHelper;

        public Factory(SqliteDataContext context, IPasswordHelper passwordHelper)
        {
            Context = context;
            _passwordHelper = passwordHelper;
            Context.SaveChanges();
        }

        public List<User> CreateUsers(int count, string password)
        {
            var users = new List<User>();
            for (var i = 0; i < count; i++)
            {
                var (passwordHash, passwordSalt) = _passwordHelper.CreateHash(password);

                users.Add(new User
                {
                    Id = i + 1,
                    Username = "user" + i,
                    FirstName = "First name " + i,
                    LastName = "Last name " + i,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                });
            }

            Context.Users.AddRange(users);
            Context.SaveChanges();
            return users;
        }
    }
}
