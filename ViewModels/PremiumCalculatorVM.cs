using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.ViewModels
{
    public class PremiumCalculatorVM
    {
        public int SchemeID { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        public decimal CalculatedPremium { get; set; }
    }
}
