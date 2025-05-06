using AutoMapper;
using AutoMapper.Configuration.Annotations;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;
namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._leaveTypeRepository = leaveTypeRepository;
            
        }
        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // retreive dinaub ebtuy object

            var target = await _leaveTypeRepository.GetByIdAsync(request.Id);

            // verify that record exists
            if (target == null) {
                throw new NotFoundException(nameof(LeaveType), request.Id);
            }

            // remove from database

            await _leaveTypeRepository.DeleteAsync(target);

            // return id
            return Unit.Value;
        }
    }
}