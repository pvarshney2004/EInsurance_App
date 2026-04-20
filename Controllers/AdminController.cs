using EInsurance_App.Data;
using EInsurance_App.Models;
using EInsurance_App.ViewModels.Customer;
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


        public IActionResult Customers()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var customers = _context.Customers
                .Select(c => new CustomerListVM
                {
                    CustomerID = c.CustomerID,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone
                }).ToList();

            return View(customers);
        }

        // for customer update
        public IActionResult EditCustomer(int id)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var customer = _context.Customers.Find(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("Password");
            ModelState.Remove("Agent");
            ModelState.Remove("Policies");
            ModelState.Remove("Payments");

            if (!ModelState.IsValid)
                return View(model);

            var customer = _context.Customers.Find(model.CustomerID);

            if (customer == null)
                return NotFound();

            // Update allowed fields only
            customer.FullName = model.FullName;
            customer.Phone = model.Phone;
            customer.DateOfBirth = model.DateOfBirth;

            _context.SaveChanges();

            return RedirectToAction("Customers");
        }

        // for cutomer delete
        public IActionResult DeleteCustomer(int id)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var customer = _context.Customers.Find(id);

            if (customer == null)
                return NotFound();

            // Check if customer has policies
            var hasPolicies = _context.Policies.Any(p => p.CustomerID == id);

            if (hasPolicies)
            {
                TempData["Error"] = "Cannot delete customer with existing policies.";
                return RedirectToAction("Customers");
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            TempData["Success"] = "Customer deleted successfully.";
            return RedirectToAction("Customers");
        }


        // for agent CRUD

        public IActionResult Agents()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var agents = _context.InsuranceAgents.ToList();

            return View(agents);
        }

        public IActionResult CreateAgent()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            return View();
        }
        [HttpPost]
        public IActionResult CreateAgent(InsuranceAgent model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("Customers");
            ModelState.Remove("Commissions");

            if (!ModelState.IsValid)
                return View(model);

            // Check duplicate email
            var exists = _context.InsuranceAgents.Any(a => a.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }

            _context.InsuranceAgents.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Agents");
        }

        public IActionResult EditAgent(int id)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var agent = _context.InsuranceAgents.Find(id);
            if (agent == null) return NotFound();

            return View(agent);
        }

        [HttpPost]
        public IActionResult EditAgent(InsuranceAgent model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("Customers");
            ModelState.Remove("Commissions");
            ModelState.Remove("Password");

            if (!ModelState.IsValid)
                return View(model);

            var agent = _context.InsuranceAgents.Find(model.AgentID);
            if (agent == null) return NotFound();

            agent.Username = model.Username;
            agent.Email = model.Email;
            agent.FullName = model.FullName;

            _context.SaveChanges();

            return RedirectToAction("Agents");
        }

        public IActionResult DeleteAgent(int id)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var agent = _context.InsuranceAgents.Find(id);
            if (agent == null) return NotFound();

            _context.InsuranceAgents.Remove(agent);
            _context.SaveChanges();

            return RedirectToAction("Agents");
        }


        // for employee CRUD
        public IActionResult Employees()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var employees = _context.Employees.ToList();

            return View(employees);
        }

        public IActionResult CreateEmployee()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            return View();
        }
        [HttpPost]
        public IActionResult CreateEmployee(Employee model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("EmployeeSchemes");

            if (!ModelState.IsValid)
                return View(model);

            var exists = _context.Employees.Any(e => e.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }

            _context.Employees.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Employees");
        }

        public IActionResult EditEmployee(int id)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var emp = _context.Employees.Find(id);
            if (emp == null) return NotFound();

            return View(emp);
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("EmployeeSchemes");
            ModelState.Remove("Password");

            if (!ModelState.IsValid)
                return View(model);

            var emp = _context.Employees.Find(model.EmployeeId);
            if (emp == null) return NotFound();

            emp.Username = model.Username;
            emp.Email = model.Email;
            emp.FullName = model.FullName;
            emp.Role = model.Role;

            _context.SaveChanges();

            return RedirectToAction("Employees");
        }

        public IActionResult DeleteEmployee(int id)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var emp = _context.Employees.Find(id);
            if (emp == null) return NotFound();

            _context.Employees.Remove(emp);
            _context.SaveChanges();

            return RedirectToAction("Employees");
        }


        // admin adding plan and scheme
        public IActionResult Plans()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var plans = _context.InsurancePlans.ToList();
            return View(plans);
        }

        public IActionResult CreatePlan()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            return View();
        }

        [HttpPost]
        public IActionResult CreatePlan(InsurancePlan model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("Schemes");

            if (!ModelState.IsValid)
                return View(model);

            _context.InsurancePlans.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Plans");
        }

        public IActionResult SchemesByPlan(int planId)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var schemes = _context.Schemes
                .Where(s => s.PlanID == planId)
                .ToList();

            ViewBag.PlanId = planId;

            return View(schemes);
        }

        public IActionResult CreateScheme(int planId)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var scheme = new Scheme
            {
                PlanID = planId
            };

            return View(scheme);
        }

        [HttpPost]
        public IActionResult CreateScheme(Scheme model)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            ModelState.Remove("Plan");
            ModelState.Remove("Policies");
            ModelState.Remove("EmployeeSchemes");

            if (!ModelState.IsValid)
                return View(model);

            _context.Schemes.Add(model);
            _context.SaveChanges();

            return RedirectToAction("SchemesByPlan", new { planId = model.PlanID });
        }


        // uc-06
        public IActionResult CommissionCalculator()
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var agents = _context.InsuranceAgents.ToList();

            return View(agents);
        }

        [HttpPost]
        public IActionResult CommissionCalculator(int agentId)
        {
            var auth = AuthorizeRole("Admin");
            if (auth != null) return auth;

            var agent = _context.InsuranceAgents
                .FirstOrDefault(a => a.AgentID == agentId);

            if (agent == null)
                return NotFound();

            var customerIds = _context.Customers
                .Where(c => c.AgentID == agentId)
                .Select(c => c.CustomerID)
                .ToList();

            var policies = _context.Policies
                .Where(p => customerIds.Contains(p.CustomerID))
                .ToList();

            List<Commission> newCommissions = new List<Commission>();

            foreach (var policy in policies)
            {
                // Checking if commission already exists
                bool exists = _context.Commissions
                    .Any(c => c.PolicyID == policy.PolicyID && c.AgentID == agentId);

                if (!exists)
                {
                    var commissionAmount = policy.Premium * 0.10m;

                    var commission = new Commission
                    {
                        AgentID = agentId,
                        PolicyID = policy.PolicyID,
                        CommissionAmount = commissionAmount
                    };

                    newCommissions.Add(commission);
                }
            }

            if (newCommissions.Any())
            {
                _context.Commissions.AddRange(newCommissions);
                _context.SaveChanges();
            }

            var totalCommission = _context.Commissions
                .Where(c => c.AgentID == agentId)
                .Sum(c => (decimal?)c.CommissionAmount) ?? 0;

            ViewBag.AgentName = agent.FullName;
            ViewBag.TotalPolicies = policies.Count;
            ViewBag.TotalCommission = totalCommission;

            return View("CommissionResult", policies);
        }
    }
}
