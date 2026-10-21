import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive, Input } from '@angular/core';

@Directive({
    selector: '[daysonappValidationRefresh]',
    providers: [{ provide: NG_VALIDATORS, useExisting: ValidationRefreshDirective, multi: true }]
})
export class ValidationRefreshDirective implements Validator {
    @Input('daysonappValidationRefresh') daysonappValidationRefresh: string;

    constructor() {
    }

    validate(control: AbstractControl): { [key: string]: any } {
        const destination = control.parent.controls[this.daysonappValidationRefresh];

        if (destination) {
            setTimeout(() => {
                // console.log('updating control: ', destination);
                destination.updateValueAndValidity();
            }, 100);
        }

        return null;
    }
}
