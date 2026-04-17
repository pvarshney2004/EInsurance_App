using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required, StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // MANY-to-MANY with Scheme
        public ICollection<EmployeeScheme> EmployeeSchemes { get; set; }

    }
}
