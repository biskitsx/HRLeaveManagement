using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        private readonly IMapper _mapper;

        private readonly IUserService _userService;
        private readonly IAppLogger<CreateLeaveAllocationCommandHandler> _logger;

        public CreateLeaveAllocationCommandHandler(
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper,
            IUserService userService,
            IAppLogger<CreateLeaveAllocationCommandHandler> logger
            )
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }


        public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any() && validationResult != null)
            {
                throw new BadRequestException(nameof(LeaveAllocation), validationResult);
            }


            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);
            if (leaveType == null)
            {
                throw new NotFoundException(nameof(LeaveType), request);
            }

            var employees = await _userService.GetEmployees();


            var period = DateTime.Now.Year;
            var allocations = new List<Domain.LeaveAllocation>();
            foreach (var emp in employees)
            {
                var allocationExists = await _leaveAllocationRepository
                    .AllocationExists(emp.Id, request.LeaveTypeId, period);

                if (allocationExists)
                {
                    continue;
                }

                allocations.Add(new Domain.LeaveAllocation
                {
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period,
                    LeaveTypeId = request.LeaveTypeId,
                    EmployeeId = emp.Id
                });

            }

            if (allocations.Count == 0)
            {
                _logger.LogWarning($"No allocations were created for {request.LeaveTypeId}");
                return Unit.Value;
            }
            await _leaveAllocationRepository.AddAllocations(allocations);

            return Unit.Value;
        }
    }
}