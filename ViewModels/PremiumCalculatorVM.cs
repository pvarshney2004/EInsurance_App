namespace EInsurance_App.ViewModels
{
    public class PremiumCalculatorVM
    {
        public int SchemeID { get; set; }
        public int Age { get; set; }
        public int Duration { get; set; }
        public decimal CalculatedPremium { get; set; }
    }
}
