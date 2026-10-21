using DaysOn.Auth.Models.ApiCommand;
using FluentValidation;

namespace DaysOn.Auth.Web.Validators
{
    public class UserActivationValidator : AbstractValidator<ActivateUserCommand>
    {
        public UserActivationValidator()
        {
            RuleFor(x => x.ActivationCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty();
        }
    }
}
