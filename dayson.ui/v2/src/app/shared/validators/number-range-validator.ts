import { AbstractControl, FormControl } from '@angular/forms';

export class NumberRangeValidator {

    public static numberRange(minimum?: number | AbstractControl, maximum?: number | AbstractControl) {
        return (control: AbstractControl): { [value: string]: any; } => {
            let max = maximum;
            if (max instanceof AbstractControl) {
                if ((<AbstractControl>maximum).value === null) {
                    max = NaN;
                } else {
                    max = Number((<AbstractControl>maximum).value);
                }
            }

            let min = minimum;
            if (min instanceof AbstractControl) {
                if ((<AbstractControl>minimum).value === null) {
                    min = NaN;
                } else {
                    min = Number((<AbstractControl>minimum).value);
                }
            }

            const cv = Number(control.value);
            if (!isNaN(cv) && ((!isNaN(max) && cv > max) || (!isNaN(min) && cv < min))) {
                return {
                    'numberRange': {
                        valid: false,
                        min: min,
                        max: max
                    }
                };
            }

            return null;
        };
    }

}
