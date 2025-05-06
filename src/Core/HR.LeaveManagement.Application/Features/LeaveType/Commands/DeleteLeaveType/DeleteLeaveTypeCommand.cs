using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommand: IRequest<Unit> // Unit คือ ว่างเป่า 
    {
        public int Id { get; set;} 

    }
}