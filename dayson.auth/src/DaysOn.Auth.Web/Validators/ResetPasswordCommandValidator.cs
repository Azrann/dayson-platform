using DaysOn.Auth.Models.ApiCommand;
using FluentValidation;

namespace DaysOn.Auth.Web.Validators
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.ResetPasswordToken)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ApiErrors.ResetPasswordTokenIsEmptyOrInvalid);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ApiErrors.PasswordMustBeSupplied)
                .Length(6, 256)
                .WithMessage(ApiErrors.PasswordMustBeBetween6And256Characters);

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ApiErrors.ConfirmPasswordMustBeSupplied)
                .Equal(x => x.Password)
                .WithMessage(ApiErrors.PasswordAndConfirmPasswordMustMatch);
        }
    }
}