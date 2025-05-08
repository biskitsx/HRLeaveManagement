using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using MediatR;
using Microsoft.VisualBasic;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails
{
    public class GetLeaveAllocationDetailsQueryHandler : IRequestHandler<GetLeaveAllocationDetailsQuery, List<LeaveAllocationDetailsDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        public GetLeaveAllocationDetailsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }
        public async Task<List<LeaveAllocationDetailsDto>> Handle(GetLeaveAllocationDetailsQuery request, CancellationToken cancellationToken)
        {

            var response = await _leaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);
            if (response == null)
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);

            var allocations = _mapper.Map<List<LeaveAllocationDetailsDto>>(response);
            return allocations;
        }
    }
}