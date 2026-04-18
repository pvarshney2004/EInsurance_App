using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.ViewModels.Policy
{
    public class BuyPolicyVM
    {
        public int SchemeID { get; set; }

        [Required]
        public string PolicyDetails { get; set; }

        [Required]
        public decimal Premium { get; set; }

        [Required]
        public int MaturityPeriod { get; set; }
    }
}
