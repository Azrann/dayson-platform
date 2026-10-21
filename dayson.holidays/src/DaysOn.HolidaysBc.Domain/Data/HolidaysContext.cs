using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Data
{
    [ExcludeFromCodeCoverage]
    public class HolidaysContext : DbContext
    {
        public HolidaysContext()
            : base("HolidaysDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rule>()
                        .HasRequired(s => s.Team)
                        .WithMany(s => s.Rules)
                        .HasForeignKey(s => s.TeamId);

            modelBuilder.Entity<HolidayRequest>()
            .HasRequired(s => s.Employee)
            .WithMany(s => s.HolidayRequests)
            .HasForeignKey(s => s.EmployeeId);

            modelBuilder.Entity<YearlyHolidayCalculation>()
            .HasRequired(s => s.Employee)
            .WithMany(s => s.YearlyHolidayCalculations)
            .HasForeignKey(s => s.EmployeeId);

            modelBuilder.Entity<Company>()
                .HasRequired(e => e.GlobalRules);
        }
    }
}
