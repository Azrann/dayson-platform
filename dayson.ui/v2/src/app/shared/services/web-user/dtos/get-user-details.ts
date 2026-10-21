import { IUserId } from '../../../data/user/user-id';
import { IWebUserDetails } from '../../../data/user/web-user-details';

export interface IGetUserDetailsRequest {
    userId: IUserId;
}

export interface IGetUserDetailsResponse {
    user: IWebUserDetails;
}
