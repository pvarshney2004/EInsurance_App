using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(15)]
        public string Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(200)]
        public string Password { get; set; }


        public int? AgentID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public InsuranceAgent Agent { get; set; }

        public ICollection<Policy> Policies { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
