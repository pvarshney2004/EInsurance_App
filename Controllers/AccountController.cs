using EInsurance_App.Data;
using EInsurance_App.Helpers;
using EInsurance_App.Models;
using EInsurance_App.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EInsurance_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AccountController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public IActionResult Register()
        {
            var role = HttpContext.Session.GetString("Role");

            // If already logged in -> don't allow register page
            if (!string.IsNullOrEmpty(role))
            {
                return RedirectToAction("RedirectDashboard");
            }
            return View();
        }

        public IActionResult Login()
        {
            var role = HttpContext.Session.GetString("Role");

            // If already logged in -. redirect to dashboard
            if (!string.IsNullOrEmpty(role))
            {
                return RedirectToAction("RedirectDashboard");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterCustomerVM registerCustomerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerCustomerVM);
            }
            var exists = _context.Customers.Any(x => x.Email == registerCustomerVM.Email) ||
                            _context.Admins.Any(x => x.Email == registerCustomerVM.Email) ||
                            _context.Employees.Any(x => x.Email == registerCustomerVM.Email) ||
                            _context.InsuranceAgents.Any(x => x.Email == registerCustomerVM.Email);

            if (exists)
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(registerCustomerVM);
            }

            var customer = new Customer
            {
                FullName = registerCustomerVM.FullName,
                Email = registerCustomerVM.Email,
                Phone = registerCustomerVM.Phone,
                DateOfBirth = registerCustomerVM.DateOfBirth,
                Password = PasswordHasher.Hash(registerCustomerVM.Password),
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string role = null;
            string email = null;

            // CUSTOMER
            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == model.Email);

            if (customer != null && PasswordHasher.Verify(model.Password, customer.Password))
            {
                role = "Customer";
                email = customer.Email;
            }
            else
            {
                // ADMIN
                var admin = _context.Admins
                    .FirstOrDefault(x => x.Email == model.Email);

                if (admin != null && admin.Password == model.Password)
                {
                    role = "Admin";
                    email = admin.Email;
                }
                else
                {
                    // EMPLOYEE
                    var employee = _context.Employees
                        .FirstOrDefault(x => x.Email == model.Email);

                    if (employee != null && employee.Password == model.Password)
                    {
                        role = "Employee";
                        email = employee.Email;
                    }
                    else
                    {
                        // AGENT
                        var agent = _context.InsuranceAgents
                            .FirstOrDefault(x => x.Email == model.Email);

                        if (agent != null && agent.Password == model.Password)
                        {
                            role = "Agent";
                            email = agent.Email;
                        }
                    }
                }
            }

            if (role == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            var token = GenerateJwtToken(email, role);

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("Role", role);
            HttpContext.Session.SetString("UserEmail", email);

            return role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Agent" => RedirectToAction("Dashboard", "Agent"),
                "Customer" => RedirectToAction("Dashboard", "Customer"),
                _ => RedirectToAction("Dashboard", "Employee")
            };
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  
            return RedirectToAction("Login", "Account");
        }

        private string GenerateJwtToken(string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult RedirectDashboard()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
                return RedirectToAction("Login");

            return role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Agent" => RedirectToAction("Dashboard", "Agent"),
                "Customer" => RedirectToAction("Dashboard", "Customer"),
                _ => RedirectToAction("Dashboard", "Employee")
            };
        }
    }
}
