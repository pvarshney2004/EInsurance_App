using EInsurance_App.Data;
using EInsurance_App.ViewModels;
using EInsurance_App.ViewModels.Agent;
using EInsurance_App.ViewModels.Commission;
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

            var totalCustomers = _context.Customers
                .Count(c => c.AgentID == agent.AgentID);

            var totalPolicies = _context.Policies
                .Count(p => p.Customer.AgentID == agent.AgentID);

            var totalCommission = _context.Commissions
                .Where(c => c.AgentID == agent.AgentID)
                .Sum(c => (decimal?)c.CommissionAmount) ?? 0;

            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalPolicies = totalPolicies;
            ViewBag.TotalCommission = totalCommission;

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

        public IActionResult CommissionHistory()
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var agent = _context.InsuranceAgents
                .FirstOrDefault(x => x.Email == email);

            if (agent == null)
                return RedirectToAction("Login", "Account");

            var commissions = _context.Commissions
                .Where(c => c.AgentID == agent.AgentID)
                .Select(c => new CommissionHistoryVM
                {
                    CommissionID = c.CommissionID,
                    PolicyID = c.Policy.PolicyID,
                    CustomerName = c.Policy.Customer.FullName,
                    Premium = c.Policy.Premium,
                    CommissionAmount = c.CommissionAmount,
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            ViewBag.AgentName = agent.FullName;

            return View(commissions);
        }

        public IActionResult Customers()
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var agent = _context.InsuranceAgents
                .FirstOrDefault(x => x.Email == email);

            if (agent == null)
                return RedirectToAction("Login", "Account");

            var customers = _context.Customers
                .Where(c => c.AgentID == agent.AgentID)
                .Select(c => new CustomerListVM
                {
                    CustomerID = c.CustomerID,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    PolicyCount = c.Policies.Count()
                })
                .ToList();

            ViewBag.AgentName = agent.FullName;

            return View(customers);
        }

        public IActionResult CustomerPolicies(int customerId)
        {
            var auth = AuthorizeRole("Agent");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var agent = _context.InsuranceAgents
                .FirstOrDefault(x => x.Email == email);

            if (agent == null)
                return RedirectToAction("Login", "Account");

            var isValidCustomer = _context.Customers
                .Any(c => c.CustomerID == customerId && c.AgentID == agent.AgentID);

            if (!isValidCustomer)
                return Unauthorized();

            var policies = _context.Policies
                .Where(p => p.CustomerID == customerId)
                .Select(p => new ViewModels.Agent.CustomerPolicyVM
                {
                    PolicyID = p.PolicyID,
                    Premium = p.Premium,
                    DateIssued = p.DateIssued,
                    SchemeName = p.Scheme.SchemeName
                })
                .ToList();

            return View(policies);
        }

    }
}
