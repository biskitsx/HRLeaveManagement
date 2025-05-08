using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList
{
    public class GetLeaveRequestListQueryHandler : IRequestHandler<GetLeaveRequestListQuery, List<LeaveRequestListDto>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetLeaveRequestListQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _userService = userService;
        }
        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
        {

            var leaveReq = new List<Domain.LeaveRequest>();
            var leaveReqDto = new List<LeaveRequestListDto>();

            var userId = _userService.UserId;
            if (userId != null)
            {
                // Get leave requests for the logged-in user
                leaveReq = await _leaveRequestRepository.GetLeaveRequestsWithDetails(userId);


                var employee = await _userService.GetEmployee(userId);
                if (employee == null)
                {
                    throw new NotFoundException(nameof(employee), userId);
                }


                leaveReqDto = _mapper.Map<List<LeaveRequestListDto>>(leaveReq);
                foreach (var req in leaveReqDto)
                {
                    req.Employee = employee;
                }
                return leaveReqDto;

            }
            else
            {
                leaveReq = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
                leaveReqDto = _mapper.Map<List<LeaveRequestListDto>>(leaveReq);
                foreach (var req in leaveReqDto)
                {
                    var employee = await _userService.GetEmployee(req.RequestingEmployeeId);
                    if (employee == null)
                    {
                        throw new NotFoundException(nameof(employee), req.RequestingEmployeeId);
                    }
                    req.Employee = employee;
                }
            }
            return leaveReqDto;
        }
    }
}