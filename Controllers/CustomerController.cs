using EInsurance_App.Data;
using EInsurance_App.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;

namespace EInsurance_App.Controllers
{
    public class CustomerController : BaseController
    {
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            return View();
        }
    }
}
