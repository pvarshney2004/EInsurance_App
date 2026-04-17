using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public int CustomerID { get; set; }
        public int PolicyID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Customer Customer { get; set; }
        public Policy Policy { get; set; }
    }
}
