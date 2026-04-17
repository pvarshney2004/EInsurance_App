using EInsurance_App.Data;
using EInsurance_App.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EInsurance_App.Controllers
{
    public class AdminController : BaseController
    {
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            return View();
        }
    }
}
