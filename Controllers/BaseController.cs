using Microsoft.AspNetCore.Mvc;

namespace EInsurance_App.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult AuthorizeRole(string role)
        {
            var userRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userRole))
                return RedirectToAction("Login", "Account");

            if (userRole != role)
                return RedirectToAction("Login", "Account");

            return null;
        }
    }
}
