using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.ViewModels
{
    public class PremiumCalculatorVM
    {
        public int SchemeID { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "Age must be greater than 0")]
        public int Age { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Duration must be greater than 0")]
        public int Duration { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Coverage amount must be greater than 0")]
        public decimal CoverageAmount { get; set; }

        public decimal CalculatedPremium { get; set; }
    }
}
