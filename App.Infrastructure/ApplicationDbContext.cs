
using App.Domain.Models.Employees;
using App.Domain.Models.Identity;
using App.Domain.Models.Salaries;
using App.Infrastructure.Configurations.Global;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            InitializeContext();
        }

    
        protected virtual void InitializeContext()
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            GlobalEntitySettings.ApplyGlobalQueryFilters(builder);
            foreach (var fk in builder.Model.GetEntityTypes()
                                            .SelectMany(e => e.GetForeignKeys())
                                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
            {
                fk.DeleteBehavior = DeleteBehavior.NoAction; 
            }

            
        }

        public DbSet<Employee> employees { get; set; }
		public DbSet<EmployeeSalaries> employeeSalaries { get; set; }


	}

}
