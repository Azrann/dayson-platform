import { AbstractControl, FormControl } from '@angular/forms';

export class ValidationRefresher {

    public static refresh(target: AbstractControl | AbstractControl[]) {
        return (control: AbstractControl): { [value: string]: any; } => {
            if (target instanceof Array) {
                target.forEach(t => {
                    t.updateValueAndValidity();
                });
            } else if (target) {
                target.updateValueAndValidity();
            }

            return null;
        };
    }

}
