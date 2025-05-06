using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;

namespace HR.LeaveManagement.Persistence.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            var now = DateTime.Now;
            builder.HasData(
              new LeaveType
              {
                  Id = 1,
                  Name = "Vacation",
                  DefaultDays = 10,
                  DateCreated = now,
                  DateModified = now
              }
            );

        }
    }
}