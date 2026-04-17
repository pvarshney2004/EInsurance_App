using EInsurance_App.Data;
using EInsurance_App.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EInsurance_App.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            return View();
        }
        [HttpGet]
        public IActionResult CustomerPolicies(string email)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            if (string.IsNullOrEmpty(email))
                return View();

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
            {
                ViewBag.Error = "Customer not found";
                return View();
            }

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
