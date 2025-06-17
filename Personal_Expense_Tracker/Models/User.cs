namespace Personal_Expense_Tracker.Models;

    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Profession { get; set; }
        public required string SalaryRange { get; set; }
        public required string PasswordHash { get; set; }
    public ICollection<Expense> Expenses { get; set; }
           = new List<Expense>();
}

