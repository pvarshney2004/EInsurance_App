using System.ComponentModel.DataAnnotations;

namespace EInsurance_App.Models
{
    public class EmployeeScheme
    {
        [Key]
        public int EmployeeSchemeID { get; set; }
        public int EmployeeID { get; set; }
        public int SchemeID { get; set; }

        // Navigation
        public Employee Employee { get; set; }
        public Scheme Scheme { get; set; }
    }
}
