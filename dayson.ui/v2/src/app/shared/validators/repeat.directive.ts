import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive, Input } from '@angular/core';

@Directive({
    selector: '[daysonappRepeat]',
    providers: [{ provide: NG_VALIDATORS, useExisting: RepeatDirective, multi: true }]
})
export class RepeatDirective implements Validator {
    @Input('daysonappRepeat') daysonappRepeat: string;

    constructor() {
    }

    validate(control: AbstractControl): { [key: string]: any } {
        const sourceValue = control.parent.value[this.daysonappRepeat];

        // console.log('RepeatDirective.validate ', control.value, sourceValue);
        if (!control.value || !sourceValue) {
            return null;
        }

        if (control.value !== sourceValue) {
            return { 'repeat': true };
        }

        return null;
    }
}
