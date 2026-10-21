using DaysOn.Auth.Models.ApiCommand;
using FluentValidation;

namespace DaysOn.Auth.Web.Validators
{
    public class ResetPasswordCodeCommandValidator : AbstractValidator<ResetPasswordCodeCommand>
    {
        public ResetPasswordCodeCommandValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
        }
    }
}