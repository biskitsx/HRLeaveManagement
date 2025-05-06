using FluentValidation.Results;

namespace HR.LeaveManagement.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, ValidationResult validationResult) : base(message)
        {

        }
        
    }
}