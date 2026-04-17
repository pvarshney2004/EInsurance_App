using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Commission
    {
        [Key]
        public int CommissionID { get; set; }
        public int AgentID { get; set; }
        public int PolicyID { get; set; }

        [Required]
        public decimal CommissionAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public InsuranceAgent Agent { get; set; }
        public Policy Policy { get; set; }
    }
}
