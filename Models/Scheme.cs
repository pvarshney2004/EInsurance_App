using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class Scheme
    {
        [Key]
        public int SchemeID { get; set; }

        [Required, StringLength(100)]
        public string SchemeName { get; set; }

        [Required]
        public string SchemeDetails { get; set; }

        // FK → Plan
        public int PlanID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public InsurancePlan Plan { get; set; }

        public ICollection<Policy> Policies { get; set; }

        public ICollection<EmployeeScheme> EmployeeSchemes { get; set; }
    }
}
