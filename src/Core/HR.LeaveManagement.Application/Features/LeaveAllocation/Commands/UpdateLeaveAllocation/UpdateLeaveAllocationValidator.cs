using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationValidator : AbstractValidator<UpdateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public UpdateLeaveAllocationValidator(ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;

            RuleFor(p => p.Id)
                .NotNull().WithMessage("{PropertyName} is required")
                .MustAsync(LeaveAllocationExists)
                .NotEmpty()
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

            RuleFor(p => p.NumberOfDays)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty();

            RuleFor(p => p.Period)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty()
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(p => p.LeaveTypeId)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty()
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(q => q)
                .MustAsync(LeaveTypeExists)
                .WithMessage("Leave type does not exist");
        }

        private async Task<bool> LeaveAllocationExists(int arg1, CancellationToken token)
        {
            var item = await _leaveAllocationRepository.GetByIdAsync(arg1);
            if (item == null)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> LeaveTypeExists(UpdateLeaveAllocationCommand command, CancellationToken token)
        {
            var item = await _leaveTypeRepository.GetByIdAsync(command.LeaveTypeId);
            if (item == null)
            {
                return false;
            }
            return true;
        }
    }
}