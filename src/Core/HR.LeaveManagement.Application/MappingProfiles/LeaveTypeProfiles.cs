using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetail;
using HR.LeaveManagement.Domain;
namespace HR.LeaveManagement.Application.MappingProfiles
{
    public class LeaveTypeProfiles: Profile
    {
        public LeaveTypeProfiles()
        {
            CreateMap<LeaveTypeDto, LeaveType>().ReverseMap(); // two way mapping
            CreateMap<LeaveType, LeaveTypeDetailDto>(); // one way !!! 1 ไป 2 เท่านั้น กลับไม่ได้

        }
    }
}