using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetail
{
    public record GetLeaveTypesDetailQuery(int Id): IRequest<LeaveTypeDetailDto>;
}