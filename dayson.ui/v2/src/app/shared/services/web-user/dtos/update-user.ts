import { IWebUserDetails } from '../../../data/user/web-user-details';
import { IUserDetailUpdate } from './user-detail-update';

export interface IUpdateUserRequest extends IUserDetailUpdate {
    _IUpdateUserRequest?: any;
}

export interface IUpdateUserResponse {
    user: IWebUserDetails;
}
