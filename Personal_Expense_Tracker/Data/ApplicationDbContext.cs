// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Personal_Expense_Tracker.Models;

namespace Personal_Expense_Tracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions opts)
            : base(opts) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        protected override void OnModelCreating(ModelBuilder mb)
        {

            base.OnModelCreating(mb);

            mb.Entity<Expense>()
              .HasOne(e => e.User)
              .WithMany(u => u.Expenses)
              .HasForeignKey(e => e.UserId)
              .IsRequired();

            mb.Entity<Expense>()
              .Property(e => e.Amount)
              .HasPrecision(18, 2);
        }

    }

}

