import { Observable } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { AuthHttp } from 'angular2-jwt';

import { IUserBasicInfo } from '../auth-user/data/AuthUser';
import { IGlobalCompanyRules } from '../company/data/IGlobalCompanyRules';
import { ComService } from '../infrastructure/com-service';
import { IApprover } from './data/IApprover';
import { ITeamOverview } from './data/ITeamOverview';
import { IUserOverview } from './data/IUserOverview';
import { UserWorkingDays } from './data/UserWorkingDays';
import { IUserGetTeamsOverviewResponseDto } from './dtos/UserGetTeamsOverviewDtos';
import { UserWorkingDaysData } from './helpers/UserWorkingDays';
import { IUserGlobalRules } from '../company/data/IUserGlobalRules';
import { EventEmitter } from 'selenium-webdriver';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ReplaySubject } from 'rxjs/ReplaySubject';

@Injectable()
export class UserService extends ComService {
    public teamsOverviewChanged: ReplaySubject<IUserGetTeamsOverviewResponseDto>;

    constructor(
        http: Http,
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public getAvailableWorkingDays(): UserWorkingDaysData[] {
        return <UserWorkingDaysData[]>[
            <UserWorkingDaysData>{ id: UserWorkingDays.monFri, name: 'Monday - Fridays', shortName: 'Mon-Fri' },
            <UserWorkingDaysData>{ id: UserWorkingDays.monSat, name: 'Monday - Saturdays', shortName: 'Mon-Sat' },
            <UserWorkingDaysData>{ id: UserWorkingDays.weekends, name: 'Weekends', shortName: 'Weekends' },
            <UserWorkingDaysData>{ id: UserWorkingDays.flexible, name: 'Flexible', shortName: 'Flex' },
            <UserWorkingDaysData>{ id: UserWorkingDays.other, name: 'Other', shortName: 'Other' },
        ];
    }

    public getTeamsOverview(reset: boolean = false): Observable<IUserGetTeamsOverviewResponseDto> {
        if (!this.teamsOverviewChanged) {
            this.teamsOverviewChanged = new  ReplaySubject<IUserGetTeamsOverviewResponseDto>(1);
            reset = true;
        }

        if (reset) {
            this.get<any, IUserGetTeamsOverviewResponseDto, IUserGetTeamsOverviewResponseDto>({
                endpoint: 'http://dayson-facade.gear.host/v1/user/teamsOverview',
                operationId: 'getTeamsOverview',
                useCaching: false,
                clearExistingCache: reset
            }).subscribe((teamsOverview: IUserGetTeamsOverviewResponseDto) => {
                this.teamsOverviewChanged.next(teamsOverview);
            });
        }

        return this.teamsOverviewChanged.asObservable();
    }

    public getTeam(teamId: string): Observable<ITeamOverview> {
        throw new Error(`getTeam not implemented`);
    }

    public updateTeam(team: ITeamOverview): Observable<ITeamOverview> {
        throw new Error(`updateTeam not implemented`);
    }

    public getUser(userId: string): Observable<IUserBasicInfo> {
        return this.get({
            endpoint: `http://dayson-facade.gear.host/v1/company/employee/${userId}`,
            operationId: `getuser-${userId}`,
            useCaching: true,
        });
    }

    public getRules(userId: string): Observable<IUserGlobalRules> {
        return this.get({
            endpoint: `http://dayson-facade.gear.host/v1/employee/${userId}/globalRules`,
        });
    }

    public setRules(userId: string, rules: IUserGlobalRules): Observable<IUserGlobalRules> {
        return this.post({
            endpoint: `http://dayson-facade.gear.host/v1/employee/${userId}/globalRules`,
            request: rules
        });
    }

    public updateUser(user: IUserBasicInfo): Observable<IUserBasicInfo> {
        return this.post({
            endpoint: `http://dayson-facade.gear.host/v1/company/employee/update`,
            request: user
        });
    }

    public getApprovers(teamId: string): Observable<IApprover[]> {
        return this.get({
            endpoint: 'http://dayson-facade.gear.host/v1/company/approvers/all',
        });
    }
}
