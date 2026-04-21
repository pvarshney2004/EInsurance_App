using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.ViewModels.Policy
{
    public class BuyPolicyVM
    {
        public int SchemeID { get; set; }

        public string PolicyDetails { get; set; }

        public decimal Premium { get; set; }
        public int MaturityPeriod { get; set; }

        public decimal CoverageAmount { get; set; }
        public int Age { get; set; }
    }
}
