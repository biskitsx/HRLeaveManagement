using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Commands
{
    public class GetLeaveTypeListQueryHandlerTest
    {
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
        private readonly IMapper _mapper;
        private Mock<IAppLogger<GetLeaveTypesQueryHandler>> _mockLogger;

        public GetLeaveTypeListQueryHandlerTest()
        {
            this._mockLeaveTypeRepository = MockLeaveTypeRepository.GetMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LeaveTypeProfiles>();
            });


            _mapper = mapperConfig.CreateMapper();
            _mockLogger = new Mock<IAppLogger<GetLeaveTypesQueryHandler>>();
        }


        [Fact]
        public async Task GetLeaveTypeListTest()
        {
            var handler = new GetLeaveTypesQueryHandler(_mapper, _mockLeaveTypeRepository.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
            result.Count.ShouldBe(3);
        }


    }
}