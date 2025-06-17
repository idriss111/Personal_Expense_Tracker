using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Expense_Tracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        // must match User.Id (which is an int)
        public int UserId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal Amount { get; set; }

        public string Category { get; set; }
       

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
