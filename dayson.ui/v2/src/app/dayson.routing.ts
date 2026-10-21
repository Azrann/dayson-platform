import { Route } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SignInComponent } from './login/sign-in/sign-in.component';
import { SignUpComponent } from './login/sign-up/sign-up.component';

export const DaysonRoutes: Route[] = [
    { path: '', redirectTo: 'login/signin', pathMatch: 'full' },
    { path: '**', redirectTo: 'login/signin', pathMatch: 'full' }
];