namespace TimeBasedOTPBT.Models.Totp
{
    public class TotpModel
    {
        public string TotpPassword { get; set; }
        public int RemainingTime { get; set; }
    }
}
