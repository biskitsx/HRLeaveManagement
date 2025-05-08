using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public CreateLeaveAllocationCommandValidator(
            ILeaveTypeRepository leaveTypeRepository)
        {

            _leaveTypeRepository = leaveTypeRepository;
            RuleFor(p => p.LeaveTypeId)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.");


            RuleFor(p => p)
                .MustAsync(async (x, cancellation) => await LeaveTypeExists(x.LeaveTypeId))
                .WithMessage("{PropertyName} does not exist.");
        }

        private async Task<bool> LeaveTypeExists(int leaveTypeId)
        {
            var item = await _leaveTypeRepository.GetByIdAsync(leaveTypeId);
            return item != null;
        }
    }


}