using Microsoft.AspNetCore.Mvc;

namespace EInsurance_App.Controllers
{
    public class AgentController : BaseController
    {
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            return View();
        }
    }
}
