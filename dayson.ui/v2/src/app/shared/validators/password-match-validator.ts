import { AbstractControl } from '@angular/forms';

export class PasswordMatchValidator {

    /**
     * Returns a validation method that will set the passwordMatch attribute
     *
     * @param password Other password field that should match the current one.
     */
    public static passwordMatch(password: AbstractControl) {
        return (repeatPasword: AbstractControl): { [key: string]: any; } => {
            return (password.value === repeatPasword.value) ? null : { 'passwordMatch': { valid: false } };
        };
    }

}
