using EInsurance_App.Models;

namespace EInsurance_App.ViewModels.Policy
{
    public class AvailablePolicyVM
    {
        public string PlanName { get; set; }
        public List<SchemeVM> Schemes { get; set; }
    }
}
