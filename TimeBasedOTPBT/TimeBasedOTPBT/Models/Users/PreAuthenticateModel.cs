using System.ComponentModel.DataAnnotations;

namespace TimeBasedOTPBT.Models.Users
{
    public class PreAuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}