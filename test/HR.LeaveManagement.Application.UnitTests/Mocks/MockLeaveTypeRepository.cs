using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Persistence;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetMockLeaveTypeRepository()
        {
            var leaveTypes = new List<Domain.LeaveType>()
            {
                new Domain.LeaveType()
                {
                    Id = 1,
                    DefaultDays = 10,
                    Name = "Test Leave Type 1"
                },
                new Domain.LeaveType()
                {
                    Id = 2,
                    DefaultDays = 15,
                    Name = "Test Leave Type 2"
                },
                new Domain.LeaveType()
                {
                    Id = 3,
                    DefaultDays = 20,
                    Name = "Test Leave Type 3"
                }
            };

            var mockRepo = new Mock<ILeaveTypeRepository>();
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(leaveTypes);
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.LeaveType>()))
                .Returns((Domain.LeaveType leaveType) =>
                {
                    leaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });

            mockRepo.Setup(r => r.IsLeaveTypeUnique(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                {
                    return !leaveTypes.Any(q => q.Name == name);
                });

            return mockRepo;
        }

    }
}