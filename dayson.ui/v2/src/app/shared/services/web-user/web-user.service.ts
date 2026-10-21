import { Injectable } from '@angular/core';
import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';

import { ITeamId } from '../../data/team/team-id';
import { IUserId } from '../../data/user/user-id';
import { IWebUserDetails } from '../../data/user/web-user-details';
import { IWebUserOverview } from '../../data/user/web-user-overview';
import { ComService } from '../infrastructure/com-service';
import { ICreateUserRequest, ICreateUserResponse } from './dtos/create-user';
import { IGetCurrentUserDetailsRequest, IGetCurrentUserDetailsResponse } from './dtos/get-current-user-details';
import { IGetUserDetailsRequest, IGetUserDetailsResponse } from './dtos/get-user-details';
import { ISearchUsersRequest, ISearchUsersResponse } from './dtos/search-users';
import { IUpdateUserRequest, IUpdateUserResponse } from './dtos/update-user';
import { IUserDetailUpdate } from './dtos/user-detail-update';

@Injectable()
export class WebUserService extends ComService {
    constructor(
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public searchUsers(
        text: string,
        teamIds: ITeamId[],
        onlyActive: boolean
    ): Observable<IWebUserOverview[]> {
        return this.get<ISearchUsersRequest, ISearchUsersResponse, IWebUserOverview[]>({
            endpoint: `${this.BASEURL}/WebUser/searchUsers`,
            operationId: `searchUsers`,
            request: { text, teamIds, onlyActive },
            transformer: (response) => response.users
        });
    }

    public getCurrentUserDetails(
    ): Observable<IWebUserDetails> {
        return this.get<IGetCurrentUserDetailsRequest, IGetCurrentUserDetailsResponse, IWebUserDetails>({
            endpoint: `${this.BASEURL}/WebUser/getCurrentUserDetails`,
            operationId: `getCurrentUserDetails`,
            request: null,
            useCaching: true,
            transformer: (response) => response.user
        });
    }

    public getUserDetails(
        userId: IUserId
    ): Observable<IWebUserDetails> {
        return this.get<IGetUserDetailsRequest, IGetUserDetailsResponse, IWebUserDetails>({
            endpoint: `${this.BASEURL}/WebUser/getUserDetails`,
            operationId: `getUserDetails`,
            request: { userId },
            transformer: (response) => response.user
        });
    }

    public updateUser(
        userId: IUserId,
        userDetailUpdate: IUserDetailUpdate
    ): Observable<IWebUserDetails> {
        userDetailUpdate.userId = userId;

        return this.post<IUpdateUserRequest, IUpdateUserResponse, IWebUserDetails>({
            endpoint: `${this.BASEURL}/WebUser/updateUser`,
            operationId: `updateUser`,
            request: userDetailUpdate as any,
            transformer: (response) => response.user
        });
    }

    public createUser(
        userDetailUpdate: IUserDetailUpdate
    ): Observable<IWebUserDetails> {
        return this.post<ICreateUserRequest, ICreateUserResponse, IWebUserDetails>({
            endpoint: `${this.BASEURL}/WebUser/createUser`,
            operationId: `createUser`,
            request: userDetailUpdate as any,
            transformer: (response) => response.user
        });
    }
}
