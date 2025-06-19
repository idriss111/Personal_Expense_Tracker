using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Personal_Expense_Tracker.Data;
using Personal_Expense_Tracker.Models;
using Personal_Expense_Tracker.Views.Home;
using System.Collections.Generic;
using System.Diagnostics;

namespace Personal_Expense_Tracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public HomeController(ApplicationDbContext db,
                              IPasswordHasher<User> hasher, ILogger<HomeController> logger)
        {
            _db = db;
            _hasher = hasher;
            _logger = logger;
        }

       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Register()
        {
            var vm = new RegisterModel
            {
                FirstName = "",
                LastName = "",
                Profession = "",
                SalaryRange = "",
                Password = ""
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var hashed = _hasher.HashPassword(null, vm.Password);

           
            var user = new User

            {
                
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Profession = vm.Profession,
                SalaryRange = vm.SalaryRange,
                PasswordHash = hashed
            };

            // save
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("FirstName", vm.FirstName);
            HttpContext.Session.SetString("LastName", vm.LastName);
            HttpContext.Session.SetString("Profession", vm.Profession);
            HttpContext.Session.SetString("SalaryRange", vm.SalaryRange);
            HttpContext.Session.SetInt32("UserId", user.Id);

            // signal success
            vm.RegistrationSuccess = true;
            ModelState.Clear();   // clear out form fields
            return RedirectToAction("Welcome");
        }

        [HttpGet]
        public async Task<IActionResult> Welcome()
        {
            // 1) Get the logged‐in user’s ID from session
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                // Not in session or not a valid integer → back to login
                return RedirectToAction("Login");
            }

            // 2) Build the dashboard view model
            var dash = new DashboardViewModel
            {
                Welcome = new WelcomeViewModel
                {
                    FirstName = HttpContext.Session.GetString("FirstName") ?? "",
                    LastName = HttpContext.Session.GetString("LastName") ?? "",
                    Profession = HttpContext.Session.GetString("Profession") ?? "",
                    SalaryRange = HttpContext.Session.GetString("SalaryRange") ?? ""
                },

                // 3) Pre-populate a new expense with today's date
                NewExpense = new ExpenseViewModel
                {
                    Date = DateTime.Today
                },

                // 4) Fetch all existing expenses for this user
                ExistingExpenses = await _db.Expenses
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.Date)
                    .Select(e => new ExpenseViewModel
                    {
                        Id = e.Id,
                        Date = e.Date,
                        Category = e.Category,
                        Amount = e.Amount
                    })
                    .ToListAsync()
            };

            return View(dash);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExpense([Bind(Prefix = "NewExpense")] ExpenseViewModel newExpense)
        {
      _logger.LogInformation("Validation errors: {@Errors}",
           ModelState
             .Where(kvp => kvp.Value.Errors.Any())
              .Select(kvp => new {
               Field = kvp.Key,
               Errors = kvp.Value.Errors.Select(e => e.ErrorMessage)
             }));

            if (!ModelState.IsValid)
                return RedirectToAction("Welcome");

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
               
                return RedirectToAction("Login");
            }

            _logger.LogInformation("Logged in UserId={UserId}", userId);

            var exp = new Expense
            {
                UserId = userId,
                Date = newExpense.Date,
                Category = newExpense.Category,
                Amount = newExpense.Amount,
            };

            _db.Expenses.Add(exp);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Inserted Expense Id={Id}", exp.Id);

            return RedirectToAction("Welcome");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login");
            }

            var expense = await _db.Expenses
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                _logger.LogWarning("Delete attempt failed: Expense not found or doesn't belong to user.");
                return RedirectToAction("Welcome"); // or show error
            }

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deleted Expense Id={Id} for UserId={UserId}", expense.Id, userId);

            return RedirectToAction("Welcome");
        }


        [HttpGet]
        public IActionResult Login()
        {
            // Just render an empty LoginViewModel
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel lm)
        {
            if (!ModelState.IsValid)
                return View(lm);

            // 1) Find by first & last name
            var user = await _db.Users
                .SingleOrDefaultAsync(u =>
                    u.FirstName == lm.FirstName &&
                    u.LastName == lm.LastName);

            if (user == null)
            {
                ModelState.AddModelError("", "Account not found.");
                return View(lm);
            }

            // 2) Verify the password
            var result = _hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                lm.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid password.");
                return View(lm);
            }

            
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Profession", user.Profession ?? "");
            HttpContext.Session.SetString("SalaryRange", user.SalaryRange ?? "");
            HttpContext.Session.SetString("UserId", user.Id.ToString());


            return RedirectToAction("Welcome");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


    }
}
