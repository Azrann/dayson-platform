using DaysOn.Auth.Models.ApiCommand;
using FluentValidation;

namespace DaysOn.Auth.Web.Validators
{
    public class UserRegistrationValidator : AbstractValidator<RegisterUserCommand>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ApiErrors.EmailMustBeSupplied)
                .EmailAddress()
                .WithMessage(ApiErrors.EmailMustBeAValidEmailAddress);

        }
    }
}
