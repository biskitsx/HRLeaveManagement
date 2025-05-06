using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandValidator: AbstractValidator<UpdateLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must be fewer than 100");

            RuleFor(p => p.DefaultDays) 
                .LessThan(100).WithMessage("{PropertyName} cannot exceed 100") 
                .GreaterThan(1).WithMessage("{PropertyName} cannot be less than 1");
            this._leaveTypeRepository = leaveTypeRepository;
        }   

    }
}