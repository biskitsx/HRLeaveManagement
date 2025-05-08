using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shouldly;

namespace HR.LeaveManagement.Persistence.IntegrationTests
{
    public class HrDatabaseContextTests
    {

        private readonly HrDatabaseContext _hrDatabaseContext;
        public HrDatabaseContextTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _hrDatabaseContext = new HrDatabaseContext(dbOptions);
        }

        [Fact]
        public async Task Save_SetDateCreatedValue()
        {
            var leaveType = new LeaveType
            {
                Name = "Test Leave Type",
                DefaultDays = 10,
                DateCreated = DateTime.Now
            };
            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();


            leaveType.DateModified.ShouldNotBe(null);
        }
        [Fact]
        public async Task Save_SetDateModifiedValue()
        {
            var leaveType = new LeaveType
            {
                Name = "Test Leave Type",
                DefaultDays = 10,
                DateCreated = DateTime.Now
            };
            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();



            leaveType.Name = "Updated Leave Type";
            await _hrDatabaseContext.SaveChangesAsync();
            Assert.NotEqual(leaveType.DateCreated, leaveType.DateModified);

            leaveType.DateModified.ShouldNotBe(null);
        }
    }
}