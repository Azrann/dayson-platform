import { IWebUserDetails } from '../../../data/user/web-user-details';
import { IUserDetailUpdate } from './user-detail-update';

export interface ICreateUserRequest extends IUserDetailUpdate {
    _ICreateUserRequest?: any;
}

export interface ICreateUserResponse {
    user: IWebUserDetails;
}
