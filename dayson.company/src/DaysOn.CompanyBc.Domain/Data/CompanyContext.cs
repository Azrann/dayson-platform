using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using DaysOn.CompanyBc.BoundedContext.Entities;

namespace DaysOn.CompanyBc.BoundedContext.Data
{
    [ExcludeFromCodeCoverage]
    public class CompanyContext : DbContext
    {
        public CompanyContext()
            : base("CompanyDB")
        {
        }

      /*  public DbSet<Company> Companies { get; set; }
        public DbSet<Team> Teams { get; set; }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one-to-many 
            modelBuilder.Entity<Employee>()
                        .HasRequired<Company>(s => s.Company)
                        .WithMany(s => s.Employees)
                        .HasForeignKey(s => s.CompanyId);

            modelBuilder.Entity<Employee>()
                        .HasOptional<Team>(s => s.MyTeam)
                        .WithMany(s => s.TeamMembers)
                        .HasForeignKey(s => s.MyTeamId);

            modelBuilder.Entity<Team>()
                .HasOptional<Employee>(e => e.Manager)
                .WithMany(emp => emp.ManagerOf)
                .HasForeignKey(s => s.ManagerId);

            modelBuilder.Entity<Employee>()
                .HasMany<Team>(e => e.ManagerOf)
                .WithOptional(emp => emp.Manager)
                .HasForeignKey(s => s.ManagerId);

            modelBuilder.Entity<Team>()
            .HasOptional<Employee>(e => e.TemporalManager)
            .WithMany(emp => emp.TemporalManagerOf)
            .HasForeignKey(s => s.TemporalManagerId);


            modelBuilder.Entity<Employee>()
                .HasMany<Team>(e => e.TemporalManagerOf)
                .WithOptional(emp => emp.TemporalManager)
                .HasForeignKey(s => s.TemporalManagerId);
        }
    }
}
