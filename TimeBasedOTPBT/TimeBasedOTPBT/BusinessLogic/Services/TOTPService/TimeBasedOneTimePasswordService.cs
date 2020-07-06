using System;
using TimeBasedOTPBT.BusinessLogic.Services.HOTPService;
using TimeBasedOTPBT.Models.Totp;

namespace TimeBasedOTPBT.BusinessLogic.Services.TOTPService
{
    public class TimeBasedOneTimePasswordService : ITimeBasedOneTimePasswordService
    {
        public static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        IHasedOneTimePasswordService _hotpService;

        public TimeBasedOneTimePasswordService(IHasedOneTimePasswordService hotpService)
        {
            _hotpService = hotpService;
        }

        public TotpModel GetPassword(int secret)
        {
            //return GetPassword(secret.ToString(), GetCurrentCounter());
            return new TotpModel();
        }

        private string GetPassword(string secret, long counter, int digits = 6)
        {
            return _hotpService.GeneratePassword(secret, counter, digits);
        }

        private long GetCurrentCounter()
        {
            return GetCurrentCounter(DateTime.UtcNow, UNIX_EPOCH, 30);
        }

        private long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep)
        {
            return (long)(now - epoch).TotalSeconds / timeStep;
        }

        public bool VerifyTotpPassword(string totp, int secret)
        {
            throw new NotImplementedException();
        }
    }
}
