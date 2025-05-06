using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HR.Leavemanagement.Persistence.DatabaseContext
{
    public class HrDatabaseContext: DbContext
    {
        
        public HrDatabaseContext(DbContextOptions<HrDatabaseContext> options): base(options)
        {
        }

        public DbSet<LeaveType> LeaveTypes {get; set;}
        public DbSet<LeaveAllocation> LeaveAllocations {get; set;}
        public DbSet<LeaveRequest> LeaveRequests {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // apply จาก configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDatabaseContext).Assembly);

            modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach( var entry in base.ChangeTracker.Entries<BaseEntity>()
                .Where( q => q.State == EntityState.Added || q.State == EntityState.Modified)) {

                    var now = DateTime.Now;
                    entry.Entity.DateModified = now;

                    if (entry.State == EntityState.Added) {
                        entry.Entity.DateCreated = now;
                    }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}