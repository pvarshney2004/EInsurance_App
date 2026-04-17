using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}