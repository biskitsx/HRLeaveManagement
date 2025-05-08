using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestProfiles : Profile
    {
        public LeaveRequestProfiles()
        {
            CreateMap<LeaveRequest, LeaveRequestListDto>().ReverseMap();
            CreateMap<CreateLeaveRequestCommand, LeaveRequest>();
        }
    }
}