using OtpNet;
using System;
using TimeBasedOTPBT.Models.Totp;

namespace TimeBasedOTPBT.BusinessLogic.Services.TOTPService
{
    public class TimeBasedOneTimePasswordNuGetService : ITimeBasedOneTimePasswordService
    {
        private Totp _totp;

        public TotpModel GetPassword(int secret)
        {
            _totp = new Totp(BitConverter.GetBytes(secret));
            return new TotpModel { TotpPassword = _totp.ComputeTotp(DateTime.UtcNow), RemainingTime = _totp.RemainingSeconds() };
        }

        public bool VerifyTotpPassword(string totp, int secret)
        {
            long timeWindowsUsedOut;
            _totp = new Totp(BitConverter.GetBytes(secret));

            return _totp.VerifyTotp(totp, out timeWindowsUsedOut, null);
        }
    }
}
