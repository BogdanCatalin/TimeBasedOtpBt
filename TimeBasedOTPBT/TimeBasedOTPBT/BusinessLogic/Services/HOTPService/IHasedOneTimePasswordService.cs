namespace TimeBasedOTPBT.BusinessLogic.Services.HOTPService
{
    public interface IHasedOneTimePasswordService
    {
        string GeneratePassword(string secret, long iterationNumber, int digits = 6);
    }
}
