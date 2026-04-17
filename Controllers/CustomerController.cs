using EInsurance_App.Data;
using EInsurance_App.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;

namespace EInsurance_App.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            return View();
        }
        public IActionResult MyPolicies()
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            // Fetching policies + payments
            var policies = _context.Policies
                .Where(p => p.CustomerID == customer.CustomerID)
                .Select(p => new PolicyVM
                {
                    PolicyID = p.PolicyID,
                    Premium = p.Premium,
                    DateIssued = p.DateIssued,
                    SchemeName = p.Scheme.SchemeName,

                    Payments = p.Payments.Select(pay => new PaymentVM
                    {
                        Amount = pay.Amount,
                        PaymentDate = pay.PaymentDate
                    }).ToList()
                }).ToList();

            var vm = new CustomerPolicyVM
            {
                CustomerName = customer.FullName,
                Policies = policies
            };

            return View(vm);
        }

    }
}
