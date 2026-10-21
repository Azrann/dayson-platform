using DaysOn.Auth.Models.ApiCommand;
using FluentValidation;

namespace DaysOn.Auth.Web.Validators
{
    public class AdminUserRegistrationValidator : AbstractValidator<RegisterAdminUserCommand>
    {
        public AdminUserRegistrationValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ApiErrors.EmailMustBeSupplied)
                .EmailAddress()
                .WithMessage(ApiErrors.EmailMustBeAValidEmailAddress);

            RuleFor(x => x.CompanyId)
                .NotEmpty()
                .WithMessage(ApiErrors.CompanyIdMustBeSupplied);
        }
    }
}
