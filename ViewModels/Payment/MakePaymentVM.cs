using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.ViewModels.Payment
{
    public class MakePaymentVM
    {
        public int PolicyID { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
    }
}
