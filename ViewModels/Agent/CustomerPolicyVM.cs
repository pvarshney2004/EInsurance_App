namespace EInsurance_App.ViewModels.Agent
{
    public class CustomerPolicyVM
    {
        public int PolicyID { get; set; }
        public string SchemeName { get; set; }
        public decimal Premium { get; set; }
        public DateTime DateIssued { get; set; }
    }
}
