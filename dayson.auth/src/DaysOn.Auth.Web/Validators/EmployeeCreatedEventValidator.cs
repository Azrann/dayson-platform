using DaysOn.Common.Events.Events;
using FluentValidation;

namespace DaysOn.Auth.Web.Validators
{
    public class EmployeeCreatedEventValidator : AbstractValidator<EmployeeCreatedBusEvent>
    {
        public EmployeeCreatedEventValidator()
        {
            RuleFor(x => x.EmployeeEmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ApiErrors.EmailMustBeSupplied)
                .EmailAddress()
                .WithMessage(ApiErrors.EmailMustBeAValidEmailAddress);
        }
    }
}
