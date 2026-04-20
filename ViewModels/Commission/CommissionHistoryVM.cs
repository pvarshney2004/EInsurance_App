namespace EInsurance_App.ViewModels.Commission
{
    public class CommissionHistoryVM
    {
        public int CommissionID { get; set; }
        public int PolicyID { get; set; }
        public string CustomerName { get; set; }
        public decimal Premium { get; set; }
        public decimal CommissionAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
