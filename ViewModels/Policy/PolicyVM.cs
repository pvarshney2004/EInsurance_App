using EInsurance_App.Models;

namespace EInsurance_App.ViewModels.Policy
{
    public class PolicyVM
    {
        public int PolicyID { get; set; }
        public string SchemeName { get; set; }
        public decimal Premium { get; set; }
        public DateTime DateIssued { get; set; }

        public List<PaymentVM> Payments { get; set; }
    }
}
