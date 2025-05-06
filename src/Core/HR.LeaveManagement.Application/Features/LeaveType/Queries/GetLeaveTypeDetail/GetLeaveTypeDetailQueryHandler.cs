using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetail
{
    public class GetLeaveTypeDetailQueryHandler : IRequestHandler<GetLeaveTypesDetailQuery, LeaveTypeDetailDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public GetLeaveTypeDetailQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            
        }
        public async Task<LeaveTypeDetailDto> Handle(GetLeaveTypesDetailQuery request, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);
            var data = _mapper.Map<LeaveTypeDetailDto>(leaveType);
            return data;
        }
    }
}