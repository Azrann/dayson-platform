import { IWebUserDetails } from '../../../data/user/web-user-details';

export interface IGetCurrentUserDetailsRequest {
    _IGetCurrentUserDetailsRequestPlaceHolder?: any;
}

export interface IGetCurrentUserDetailsResponse {
    user: IWebUserDetails;
}
