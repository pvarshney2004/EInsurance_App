using Microsoft.AspNetCore.Mvc;

namespace EInsurance_App.Controllers
{
    public class EmployeeController : BaseController
    {
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Employee");
            if (auth != null) return auth;

            return View();
        }
    }
}
