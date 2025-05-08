using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Logging;


// using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
// using Microsoft.VisualBasic;
// using System.ComponentModel.DataAnnotations;
// using System.Net.Mail;
// using System.Security.Claims;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IUserService _userService;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;

        public CreateLeaveRequestCommandHandler(
            IEmailSender emailSender,
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IUserService userService
            , IAppLogger<CreateLeaveRequestCommandHandler> logger
            )
        {
            _emailSender = emailSender;
            _mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveAllocationRepository = leaveAllocationRepository;
            this._userService = userService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Request", validationResult);


            var employeeId = _userService.UserId;
            // validate the allocation 
            var allocations = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);
            if (allocations == null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(
                request.LeaveTypeId), "Leave Type does not exist for this employee."
            ));
                throw new NotFoundException(nameof(allocations), validationResult);
            }

            var daysRequested = (int)(request.EndDate - request.StartDate).TotalDays + 1;
            if (daysRequested > allocations.NumberOfDays)
            {
                validationResult.Errors.Add(
                    new FluentValidation.Results.ValidationFailure(
                        nameof(request.StartDate),
                        "You do not have enough leave days remaining."
                    ));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }
            _logger.LogWarning($"day request");

            // create the leave request

            var leaveReq = _mapper.Map<Domain.LeaveRequest>(request);
            leaveReq.RequestingEmployeeId = employeeId;
            await _leaveRequestRepository.CreateAsync(leaveReq);
            _logger.LogWarning($"leave req");

            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty, /* Get email from employee record */
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                        $"has been submitted successfully.",
                    Subject = "Leave Request Submitted"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception)
            {
                //// Log or handle error,
            }

            return Unit.Value;
        }
    }
}