namespace TimeBasedOTPBT.BusinessLogic.Services.TOTPService
{
    public interface ITimeBasedOneTimePasswordService
    {
        string GetPassword(string secret);
    }
}
