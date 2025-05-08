using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            // validate

            var validator = new UpdateLeaveAllocationValidator(_leaveAllocationRepository, _leaveTypeRepository);
            var validationResult = validator.Validate(request);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid Leave Allocation", validationResult);
            }


            var existingLeaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
            if (existingLeaveAllocation is null)
            {
                throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);
            }

            _mapper.Map(request, existingLeaveAllocation); // คือการ map จาก request ไปยัง existingLeaveAllocation

            await _leaveAllocationRepository.UpdateAsync(existingLeaveAllocation);
            return Unit.Value;
        }
    }
}