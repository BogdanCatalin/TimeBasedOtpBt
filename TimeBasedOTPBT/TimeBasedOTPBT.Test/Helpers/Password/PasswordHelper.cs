using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TimeBasedOTPBT.Test.Helpers.Password
{
    public class PasswordHelper : IPasswordHelper
    {

        private readonly IStringLocalizer<PasswordHelper> _l;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHelper"/> class.
        /// </summary>
        /// <param name="l">The string localizer.</param>
        public PasswordHelper(IStringLocalizer<PasswordHelper> l)
        {
            _l = l;
        }

        /// <inheritdoc />
        public (byte[] passwordHash, byte[] passwordSalt) CreateHash(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(_l["Value cannot be empty or whitespace only string."], nameof(password));

            using var hmac = new HMACSHA512();
            return (
                passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                passwordSalt: hmac.Key);
        }

        /// <inheritdoc />
        public bool VerifyHash(string password, byte[] hash, byte[] salt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    _l["Value cannot be empty or whitespace only string."], nameof(password));
            if (hash.Length != 64)
                throw new ArgumentException(
                    _l["Invalid length of password hash (64 bytes expected)."], nameof(hash));
            if (salt.Length != 128)
                throw new ArgumentException(
                    _l["Invalid length of password salt (128 bytes expected)."], nameof(salt));

            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (var i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != hash[i])
                    return false;

            return true;
        }

        /// <inheritdoc />
        public string GenerateRandomString(int length)
        {
            return new string(Enumerable
                .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length)
                .Select(x =>
                {
                    var cryptoResult = new byte[4];
                    using (var cryptoProvider = new RNGCryptoServiceProvider())
                        cryptoProvider.GetBytes(cryptoResult);
                    return x[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(x.Length)];
                })
                .ToArray());
        }
    }
}
