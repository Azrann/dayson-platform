import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login.component';
import { LoginRoutes } from './login.routing';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        FormsModule,
        HttpModule,
        SharedModule,
        RouterModule.forChild(LoginRoutes),
    ],
    declarations: [
        LoginComponent,
        SignInComponent,
        SignUpComponent,
        ResetPasswordComponent,
        ForgotPasswordComponent,
    ],
    providers: [
    ],
})
export class LoginModule { }
