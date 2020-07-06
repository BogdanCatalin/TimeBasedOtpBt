using System.ComponentModel.DataAnnotations;

namespace TimeBasedOTPBT.Models.Users
{
    public class AuthenticateModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string TotpPassword { get; set; }
    }
}
