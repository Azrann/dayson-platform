import { IGenericResponse } from '../../IGenericResponse';

export interface ISignUpRequest {
    adminEmailAddress: string;
    companyName: string;
    firstname: string;
    lastname: string;
    phoneNumber: string;
}

export interface ISignUpResponse extends IGenericResponse {
    access_token: string;
    expires_in: string;
    token_type: string;
    error: string;
    error_description: string;
}
