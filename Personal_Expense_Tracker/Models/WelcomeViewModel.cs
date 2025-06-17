using System.ComponentModel.DataAnnotations;

namespace Personal_Expense_Tracker.Models;


public class WelcomeViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Profession { get; set; }
    public string SalaryRange { get; set; }
}

public class ExpenseViewModel
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be > 0.")]
    public decimal Amount { get; set; }

    [Required]
    public string Category { get; set; } = "";
}

