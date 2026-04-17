using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.ViewModels.Auth
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
