namespace DaysOn.Auth.Web.Validators
{
    public static class ApiErrors
    {
        public const string CompanyIdMustBeSupplied = "CompanyId must be supplied";
        public const string EmailMustBeSupplied = "Email must be supplied.";
        public const string EmailMustBeAValidEmailAddress = "Email must be a valid email address.";
        public const string PasswordMustBeBetween6And256Characters = "Password must be between 6 and 256 characters.";
        public const string PasswordMustBeSupplied = "Password must be supplied.";
        public const string ConfirmPasswordMustBeSupplied = "Confirm password must be supplied.";
        public const string PasswordAndConfirmPasswordMustMatch = "Password and confirm password must match.";
        public const string UserCouldNotBeActivated = "User {0} could not be activated";
        public const string RestaurantCouldNotBeAssociated = "Restaurant {0} could not be associated";
        public const string UserCouldNotBeFound = "UserId {0} could not be found";
        public const string UserIsAlreadyActive = "User with id {0} is already active";
        public const string UserIdIsNullOrEmpty = "User id {0} is null or empty";
        public const string UserNotAuthorized = "User not authorized";
        public const string ActivationCodeCouldNotBeFound = "Activation code {0} could not be found";
        public const string ActivationCodeHasAlreadyExpired = "Activation code {0} has already expired";
        public const string ActivationCodeIsNotValid = "Activation code is not valid";
        public const string ActivationCodeIsNotValidFormat = "Activation code is not in a valid format";
        public const string EmailCouldNotBeFound = "Reset password code could not be generated. Email {0} is not register as user.";
        public const string LanguageCodeMustBeSupplied = "Language code must be supplied.";
        public const string InvalidLanguageCode = "Language code is not valid.";
        public const string InvalidResetPasswordToken = "Reset password code {0} is invalid";
        public const string ResetPasswordTokenHasExpired = "Reset password code {0} has expired";
        public const string ResetPasswordTokenIsEmptyOrInvalid = "Reset password token is empty or invalid";
        public const string UsernameOrPasswordIncorrect = "Username or password are incorrect.";
        public const string UserNotActive = "User is not active.";
        public const string UnknownError = "An error occurred, please try again or contact the administrator.";
        public const string ClaimUserIdNotFound = "Invalid token.Claim userId not found.";
        public const string UserIdIsNotValidFormat = "User id is not in a valid format";
    }
}
