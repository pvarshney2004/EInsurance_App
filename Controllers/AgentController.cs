using EInsurance_App.Data;
using EInsurance_App.ViewModels;
using EInsurance_App.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EInsurance_App.Controllers
{
    public class AgentController : BaseController
    {
        private readonly AppDbContext _context;
        public AgentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var agent = _context.InsuranceAgents
                .FirstOrDefault(x => x.Email == email);

            if (agent == null)
                return RedirectToAction("Login", "Account");

            // Customers linked to this agent
            var totalCustomers = _context.Customers
                .Count(c => c.AgentID == agent.AgentID);

            // Policies sold by this agent
            var totalPolicies = _context.Policies
                .Count(p => p.Customer.AgentID == agent.AgentID);

            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalPolicies = totalPolicies;

            return View();
        }

        public IActionResult AvailablePolicies()
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var data = _context.InsurancePlans
                .Select(p => new AvailablePolicyVM
                {
                    PlanName = p.PlanName,
                    Schemes = p.Schemes.Select(s => new SchemeVM
                    {
                        SchemeID = s.SchemeID,
                        SchemeName = s.SchemeName,
                        SchemeDetails = s.SchemeDetails
                    }).ToList()
                }).ToList();

            return View(data);
        }

        public IActionResult PremiumCalculator(int schemeId)
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var scheme = _context.Schemes.Find(schemeId);
            if (scheme == null) return NotFound();

            var vm = new PremiumCalculatorVM
            {
                SchemeID = schemeId
            };

            ViewBag.SchemeName = scheme.SchemeName;

            return View(vm);
        }

        [HttpPost]
        public IActionResult PremiumCalculator(PremiumCalculatorVM model)
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var scheme = _context.Schemes.Find(model.SchemeID);
            if (scheme == null) return NotFound();

            decimal baseAmount = 5000;
            decimal ageFactor = 100;
            decimal interestRate = 200;

            model.CalculatedPremium =
                baseAmount +
                (model.Age * ageFactor) +
                (model.Duration * interestRate);

            ViewBag.SchemeName = scheme.SchemeName;

            return View(model); // same page 
        }

    }
}
