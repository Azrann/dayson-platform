import { ValidationRefresher } from './../../shared/validators/validation-refresher';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PasswordMatchValidator } from '../../shared/validators/password-match-validator';
import { AuthUserService } from '../../shared/services/auth-user/auth-user.service';

@Component({
    selector: 'daysonapp-login-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
    error = false;
    success = false;
    submitting = false;

    resetCode: string;
    password: string;
    repeatPassword: string;

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private authUserService: AuthUserService,
    ) { }

    ngOnInit() {
        // this.resetPasswordForm.get('repeatPassword').setValidators(
        //     PasswordMatchValidator.passwordMatch(this.resetPasswordForm.get('password'))
        // );

        // this.resetPasswordForm.get('password').setValidators([
        //     Validators.required,
        //     Validators.minLength(6),
        //     ValidationRefresher.refresh(this.resetPasswordForm.get('repeatPassword'))
        // ]);

        this.activatedRoute.params.subscribe((params) => {
            this.resetCode = params['resetCode'];
        });
    }

    reset() {
        this.submitting = true;

        this.authUserService.passwordReset(
            this.password,
            this.repeatPassword,
            this.resetCode
        ).subscribe((response) => {
            this.submitting = false;
            this.success = true;
        }, (error) => {
            this.submitting = false;
            this.error = true;
        });
    }

    goToLogin() {
        this.router.navigate(['login', 'signin']);
    }
}
