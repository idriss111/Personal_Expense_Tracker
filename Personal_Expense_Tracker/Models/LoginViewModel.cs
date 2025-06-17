using System.ComponentModel.DataAnnotations;

namespace Personal_Expense_Tracker.Models
{
    public class LoginViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
