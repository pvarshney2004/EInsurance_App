using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class InsurancePlan
    {
        [Key]
        public int PlanID { get; set; }

        [Required, StringLength(100)]
        public string PlanName { get; set; }

        [Required]
        public string PlanDetails { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // One Plan -> Many Schemes
        public ICollection<Scheme> Schemes { get; set; }
    }
}
