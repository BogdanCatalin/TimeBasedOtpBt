using TimeBasedOTPBT.Models.Totp;

namespace TimeBasedOTPBT.BusinessLogic.Services.TOTPService
{
    public interface ITimeBasedOneTimePasswordService
    {
        TotpModel GetPassword(int secret);
        bool VerifyTotpPassword(string totp, int secret);
    }
}
