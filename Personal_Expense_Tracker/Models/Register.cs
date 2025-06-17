// Models/RegisterModel.cs
using System.ComponentModel.DataAnnotations;

namespace Personal_Expense_Tracker.Models
{
    public class RegisterModel

    {
        

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        public required string Profession { get; set; }

        [Display(Name = "Yearly Salary Range")]
        public required string SalaryRange { get; set; }

        [Required, DataType(DataType.Password)]
        public required string Password { get; set; }

        
        public bool RegistrationSuccess { get; set; }
    }

    
    
}
