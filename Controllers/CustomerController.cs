using EInsurance_App.Data;
using EInsurance_App.Models;
using EInsurance_App.Services.Pdf;
using EInsurance_App.ViewModels;
using EInsurance_App.ViewModels.Payment;
using EInsurance_App.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace EInsurance_App.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public CustomerController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public IActionResult Dashboard()
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var policies = _context.Policies
                .Where(p => p.CustomerID == customer.CustomerID)
                .ToList();

            var payments = _context.Payments
                .Where(p => p.CustomerID == customer.CustomerID)
                .ToList();

            ViewBag.TotalPolicies = policies.Count;
            ViewBag.TotalPayments = payments.Count;
            ViewBag.TotalAmount = payments.Sum(p => p.Amount);

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
    .Include(p => p.Scheme)
    .Include(p => p.Payments)
    .Where(p => p.CustomerID == customer.CustomerID)
    .AsEnumerable()
    .Select(p => new PolicyVM
    {
        PolicyID = p.PolicyID,
        Premium = p.Premium,
        DateIssued = p.DateIssued,

        SchemeName = p.Scheme?.SchemeName ?? "N/A",

        CoverageAmountDisplay = p.PolicyDetails
            .Split('|')[0]
            .Trim(),

        Payments = p.Payments.Select(pay => new PaymentVM
        {
            PaymentID = pay.PaymentID,
            Amount = pay.Amount,
            PaymentDate = pay.PaymentDate
        }).ToList(),

        RemainingAmount = p.Premium - p.Payments.Sum(pay => pay.Amount)
    })
    .ToList();

            var vm = new CustomerPolicyVM
            {
                CustomerName = customer.FullName,
                Policies = policies
            };

            return View(vm);
        }

        public IActionResult AvailablePolicies()
        {
            var auth = AuthorizeRole("Customer");
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

        // buy policy
        public IActionResult BuyPolicy(int schemeId, decimal premium, int duration, decimal coverage, int age)
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var scheme = _context.Schemes.Find(schemeId);
            if (scheme == null) return NotFound();

            var vm = new BuyPolicyVM
            {
                SchemeID = schemeId,
                Premium = premium,
                MaturityPeriod = duration,
                CoverageAmount = coverage,
                Age = age
            };

            ViewBag.SchemeName = scheme.SchemeName;

            return View(vm);
        }
        [HttpPost]
        public IActionResult BuyPolicy(BuyPolicyVM model)
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            ModelState.Remove("PolicyDetails");

            if (!ModelState.IsValid)
            {
                ViewBag.SchemeName = _context.Schemes
                    .Where(s => s.SchemeID == model.SchemeID)
                    .Select(s => s.SchemeName)
                    .FirstOrDefault();

                return View(model);
            }

            var email = HttpContext.Session.GetString("UserEmail");
            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var today = DateTime.Now;
            var finalPolicyDetails =
        $"Coverage Amount: ₹{model.CoverageAmount} | " +
        $"Premium: ₹{model.Premium} | " +
        $"Duration: {model.MaturityPeriod} years";

            var policy = new Policy
            {
                CustomerID = customer.CustomerID,
                SchemeID = model.SchemeID,
                PolicyDetails = finalPolicyDetails,
                Premium = model.Premium,
                DateIssued = today,
                MaturityPeriod = model.MaturityPeriod,
                PolicyLapseDate = today.AddYears(model.MaturityPeriod)
            };

            _context.Policies.Add(policy);
            _context.SaveChanges();

            return RedirectToAction("MyPolicies");
        }


        // payment
        public IActionResult MakePayment(int policyId)
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            //Validating ownership
            var policy = _context.Policies
                .FirstOrDefault(p => p.PolicyID == policyId && p.CustomerID == customer.CustomerID);

            if (policy == null)
                return Unauthorized(); // or Redirect

            var vm = new MakePaymentVM
            {
                PolicyID = policyId
            };

            ViewBag.PolicyID = policy.PolicyID;
            ViewBag.Premium = policy.Premium;

            return View(vm);
        }

        [HttpPost]
        public IActionResult MakePayment(MakePaymentVM model)
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            if (!ModelState.IsValid)
                return View(model);

            var email = HttpContext.Session.GetString("UserEmail");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var policy = _context.Policies
                .FirstOrDefault(p => p.PolicyID == model.PolicyID && p.CustomerID == customer.CustomerID);

            if (policy == null)
                return Unauthorized();

            var payment = new Models.Payment
            {
                CustomerID = customer.CustomerID,
                PolicyID = model.PolicyID,
                Amount = model.Amount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            TempData["Success"] = "Payment successful";

            return RedirectToAction("MyPolicies");
        }

        // uc-05
        public IActionResult PremiumCalculator(int schemeId)
        {
            var auth = AuthorizeRole("Customer");
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
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            if (!ModelState.IsValid)
                return View(model);

            var scheme = _context.Schemes.Find(model.SchemeID);
            if (scheme == null) return NotFound();

            decimal baseRate = 0.02m; // 2% of coverage
            decimal ageFactor = model.Age * 50;
            decimal durationFactor = model.Duration * 100;

            var premium =
                (model.CoverageAmount * baseRate) +
                ageFactor +
                durationFactor;

            return RedirectToAction("BuyPolicy", new
            {
                schemeId = model.SchemeID,
                premium = premium,
                duration = model.Duration,
                coverage = model.CoverageAmount,
                age = model.Age
            });
        }

        // assiging agent to the customer
        public IActionResult SelectAgent()
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            // Already assigned agent
            if (customer.AgentID != null)
            {
                var agent = _context.InsuranceAgents
                    .FirstOrDefault(a => a.AgentID == customer.AgentID);

                ViewBag.AssignedAgent = agent;
            }

            // Always load agents list
            ViewBag.Agents = _context.InsuranceAgents.ToList();

            return View();
        }
        [HttpPost]
        public IActionResult SelectAgent(int agentId)
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            // Update or assign
            customer.AgentID = agentId;
            _context.SaveChanges();
            TempData["Success"] = "Agent updated successfully ✅";
            return RedirectToAction("Dashboard");
        }

        // uc-07 generating invoice
        public IActionResult GenerateInvoice(int paymentId)
        {
            var auth = AuthorizeRole("Customer");
            if (auth != null) return auth;

            var email = HttpContext.Session.GetString("UserEmail");

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var payment = _context.Payments
                .Where(p => p.PaymentID == paymentId)
                .Select(p => new
                {
                    p.PaymentID,
                    p.Amount,
                    p.PaymentDate,
                    p.PolicyID,
                    CustomerID = p.CustomerID,
                    SchemeName = p.Policy.Scheme.SchemeName
                })
                .FirstOrDefault();

            if (payment == null)
                return NotFound();

            // SECURITY CHECK
            if (payment.CustomerID != customer.CustomerID)
                return Unauthorized();

            // Generate PDF
            var document = new InvoiceDocument(
                customer.FullName,
                payment.SchemeName,
                payment.PolicyID,
                payment.Amount,
                payment.PaymentDate
            );

            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf",
                $"Invoice_{payment.PaymentID}.pdf");
        }
    }
}