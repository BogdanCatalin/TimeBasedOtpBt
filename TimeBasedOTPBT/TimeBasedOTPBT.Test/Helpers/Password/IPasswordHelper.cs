namespace TimeBasedOTPBT.Test.Helpers.Password
{
    public interface IPasswordHelper
    {
        (byte[] passwordHash, byte[] passwordSalt) CreateHash(string password);
        bool VerifyHash(string password, byte[] hash, byte[] salt);
        string GenerateRandomString(int length);
    }
}
