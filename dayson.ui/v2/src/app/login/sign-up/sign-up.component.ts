import { ICompany } from '../../shared/services/company/data/ICompany';
import { CompanyService } from '../../shared/services/company/company.service';
import { NgForm } from '@angular/forms/src/directives';
import { ValidationRefresher } from './../../shared/validators/validation-refresher';
import { PasswordMatchValidator } from './../../shared/validators/password-match-validator';
import { OnInit, Component, ViewChildren } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';
import { AuthHttp } from 'angular2-jwt';
import { FormControl, FormGroupDirective } from '@angular/forms';
import { AuthUserService } from '../../shared/services/auth-user/auth-user.service';
import { ErrorStateMatcher } from '@angular/material/core';
import { ISignUpResponse } from '../../shared/services/auth-user/dtos/SignUpDtos';

@Component({
    selector: 'daysonapp-login-sign-up',
    templateUrl: './sign-up.component.html',
    styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
    loginError: string;
    loading: boolean;

    // debug code
    initializedEmail: string;
    initializedFirstName: string;
    initializedMiddleName: string;
    initializedLastName: string;
    initializedPassword: string;
    initializedCompanyName: string;

    matcher = new PasswordMatcher();

    constructor(
        private router: Router,
        private loginService: AuthUserService,
        private authUserService: AuthUserService,
        private companyService: CompanyService,
    ) {
    }

    ngOnInit() {
        const id = Math.floor((new Date().getTime() / 1000));
        this.initializedEmail = `email${id}@gmail.com`;
        this.initializedFirstName = `firstname${id}`;
        this.initializedMiddleName = `middlename${id}`;
        this.initializedLastName = `lastname${id}`;
        this.initializedPassword = `Password${id}!!!!`;
        this.initializedCompanyName = `company${id}`;
    }

    passwordMatcher(control: FormControl, form: FormGroupDirective | NgForm): boolean {
        if (form.value.password !== form.value.repeatPassword) {
            control.setErrors({ 'repeatPassword': true });
        } else {
            control.setErrors(undefined);
        }

        const isSubmitted = form && form.submitted;
        return !!(control.invalid && (control.dirty || control.touched || isSubmitted));
    }

    signUp(form: NgForm) {
        if (form.invalid || this.loading) {
            return;
        }

        this.loginError = null;
        this.loading = true;
        this.authUserService.signUp(
            form.value.email,
            form.value.companyName,
            form.value.firstname,
            form.value.middlename,
            form.value.lastname,
            form.value.phoneNumber
        ).subscribe((result: ISignUpResponse) => {
            console.log('signup response is', result);
            this.loading = false;
            if (result.error) {
                this.loginError = `${result.error} - ${result.error_description}`;
                return;
            }

            this.router.navigateByUrl('/admin/company');
        }, (error) => {
            this.loading = false;
            this.loginError = 'Error processing request';
        });
    }
}

class PasswordMatcher implements ErrorStateMatcher {
    isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
        if (form.value.password !== form.value.repeatPassword) {
            control.setErrors({ 'repeatPassword': true });
        } else {
            control.setErrors(undefined);
        }

        const isSubmitted = form && form.submitted;
        return !!(control.invalid && (control.dirty || control.touched || isSubmitted));
    }
}
