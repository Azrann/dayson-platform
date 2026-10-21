export interface IResetPasswordRequest {
    password: string;
    confirmPassword: string;
    resetPasswordToken: string;
}
