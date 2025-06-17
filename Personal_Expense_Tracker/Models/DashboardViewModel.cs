using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Personal_Expense_Tracker.Models;


public class DashboardViewModel
{
    
    public WelcomeViewModel Welcome { get; set; }
   
    public ExpenseViewModel NewExpense { get; set; }
    public List<ExpenseViewModel> ExistingExpenses { get; set; }
}

