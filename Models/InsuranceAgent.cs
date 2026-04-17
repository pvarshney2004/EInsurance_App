using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class InsuranceAgent
    {
        [Key]
        public int AgentID { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Customer> Customers { get; set; }

        public ICollection<Commission> Commissions { get; set; }
    }
}
