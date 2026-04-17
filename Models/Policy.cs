using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Policy
    {
        [Key]
        public int PolicyID { get; set; }

        public int CustomerID { get; set; }

        public int SchemeID { get; set; }

        [Required]
        public string PolicyDetails { get; set; }

        [Required]
        public decimal Premium { get; set; }

        [Required]
        public DateTime DateIssued { get; set; }

        [Required]
        public int MaturityPeriod { get; set; }

        [Required]
        public DateTime PolicyLapseDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Customer Customer { get; set; }
        public Scheme Scheme { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public ICollection<Commission> Commissions { get; set; }
    }
}
