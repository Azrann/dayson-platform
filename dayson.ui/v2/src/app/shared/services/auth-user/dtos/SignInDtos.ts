import { IGenericResponse } from '../../IGenericResponse';

export interface ISignInRequest {
    username: string;
    password: string;
    rememberMe: boolean;
}

export interface ISignInResponse extends IGenericResponse {
    access_token: string;
    expires_in: string;
    token_type: string;
}
