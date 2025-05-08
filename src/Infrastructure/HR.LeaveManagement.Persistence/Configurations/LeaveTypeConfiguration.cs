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

            // we can not 
            var now = new DateTime(2025, 5, 6);
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