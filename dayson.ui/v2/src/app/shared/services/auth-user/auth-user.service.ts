import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';

import { ComService } from '../infrastructure/com-service';
import { IForgotPasswordRequest } from './dtos/ForgotPasswordDtos';
import { IResetPasswordRequest } from './dtos/ResetPasswordDtos';
import { ISignInRequest, ISignInResponse } from './dtos/SignInDtos';
import { ISignUpRequest, ISignUpResponse } from './dtos/SignUpDtos';
import { IUserBasicInfo } from './data/AuthUser';

@Injectable()
export class AuthUserService extends ComService {
    private currentUser: IUserBasicInfo;

    constructor(
        private http: Http,
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public getAuthUserOld(delay: boolean): Observable<IUserBasicInfo> {
        return this.get({
            endpoint: 'http://dayson-facade.gear.host/v1/company/employee',
            useCaching: true,
            operationId: 'getAuthUser',
        });
    }

    public login(userName: string, password: string, rememberMe: boolean): Observable<ISignInResponse> {
        return this.http.post('http://dayson-facade.gear.host/v1/oauth2/token', <ISignInRequest>{
            username: userName,
            password: password,
            rememberMe: rememberMe
        }).map((response) => response.json());
    }

    public signUp(
        adminEmailAddress: string,
        companyName: string,
        firstname: string,
        middlename: string,
        lastname: string,
        phoneNumber: string,

    ): Observable<ISignUpResponse> {
        // return this.http.post('http://dayson-facade.gear.host/v1/company',
        return this.http.post(`${this.BASEURL}/company`,
            <ISignUpRequest>{
                adminEmailAddress,
                companyName,
                firstname,
                middlename,
                lastname,
                phoneNumber,
            }).
            map((response) => response.json()).
            do<ISignUpResponse>((response) => {
                console.log(`auth-user.service.signup.map`, response);
                sessionStorage.setItem('dayson-token', response.access_token);
            });
    }

    public passwordReset(
        password: string,
        confirmPassword: string,
        resetPasswordToken: string,
    ): Observable<IResetPasswordRequest> {
        return this.http.post(
            'http://dayson-facade.gear.host/v1/auth/user/password/reset',
            <IResetPasswordRequest>{
                password,
                confirmPassword,
                resetPasswordToken,
            }
        ).map((response) => response.json());
    }

    public forgotPassword(
        email: string,
    ): Observable<any> {
        return this.http.post(
            'http://dayson-facade.gear.host/v1/auth/user/password/resetcode',
            <IForgotPasswordRequest>{
                email
            }
        ).map((response) => response.json());
    }

}
