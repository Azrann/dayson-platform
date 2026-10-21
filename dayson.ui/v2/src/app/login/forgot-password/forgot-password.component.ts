import { Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AuthUserService } from '../../shared/services/auth-user/auth-user.service';

@Component({
    selector: 'daysonapp-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
    emailAddress: string;

    error = false;
    success = false;
    submitted = false;
    submitting = false;

    constructor(
        private router: Router,
        private authUserService: AuthUserService
    ) { }

    ngOnInit() {
    }

    forgotPassword() {
        this.submitted = true;
        this.error = false;
        this.success = false;
        this.submitting = true;

        this.authUserService.forgotPassword(
            this.emailAddress
        ).subscribe((response) => {
            this.success = true;
            this.submitting = false;
        }, (error) => {
            this.error = true;
            this.submitting = false;
        });
    }

    goToLogin() {
        this.router.navigate(['login', 'signin']);
    }
}
