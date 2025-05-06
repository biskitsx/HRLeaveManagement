using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommand: IRequest<Unit> // Unit คือ ว่างเป่า 
    {
        public string Name { get; set;} = string.Empty;
        public int DefaultDays { get; set;} 

    }
}