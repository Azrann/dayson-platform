import { Route } from '@angular/router';
import { LoginComponent } from './login.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';

export const LoginRoutes: Route[] = [
    {
        path: 'login',
        redirectTo: 'login/signin'
    },
    {
        path: 'login',
        component: LoginComponent,
        children: [
            { path: 'signin', component: SignInComponent },
            { path: 'signup', component: SignUpComponent },
            { path: 'password/:resetCode', component: ResetPasswordComponent },
            { path: 'forgot', component: ForgotPasswordComponent },
            { path: '', redirectTo: 'login/signin', pathMatch: 'full' },
        ]
    },
];
