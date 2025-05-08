using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using HR.LeaveManagement.Domain;
namespace HR.LeaveManagement.Application.MappingProfiles
{
    public class LeaveAllocationProfiles : Profile
    {
        public LeaveAllocationProfiles()
        {
            CreateMap<LeaveAllocation, LeaveAllocationDto>().ReverseMap(); // two way mapping
            CreateMap<LeaveAllocation, LeaveAllocationDetailsDto>(); // one way !!! 1 ไป 2 เท่านั้น กลับไม่ได้
            CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
        }
    }
}