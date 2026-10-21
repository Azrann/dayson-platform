import { Injectable } from '@angular/core';
import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';

import { ITeamId } from '../../data/team/team-id';
import { IWebTeamOverview } from '../../data/team/web-team-overview';
import { IWebApproverOverview } from '../../data/user/web-approver-overview';
import { ComService } from '../infrastructure/com-service';
import { ICreateTeamRequest, ICreateTeamResponse } from './dtos/create-team';
import { IGetTeamApproversRequest, IGetTeamApproversResponse } from './dtos/get-team-approvers';
import { IGetTeamsRequest, IGetTeamsResponse } from './dtos/get-teams';
import { IUpdateTeamRequest, IUpdateTeamResponse } from './dtos/update-team';

@Injectable()
export class WebTeamService extends ComService {
    constructor(
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public createTeam(name: string): Observable<IWebTeamOverview> {
        return this.post<ICreateTeamRequest, ICreateTeamResponse, IWebTeamOverview>({
            endpoint: `${this.BASEURL}/WebTeam/createTeam`,
            operationId: `addTeam`,
            request: {name},
            transformer: (response) => response.team
        });
    }

    public getTeamApprovers(teamId: ITeamId): Observable<IWebApproverOverview[]> {
        return this.get<IGetTeamApproversRequest, IGetTeamApproversResponse, IWebApproverOverview[]>({
            endpoint: `${this.BASEURL}/WebTeam/getTeamApprovers`,
            operationId: `getTeamApprovers-${teamId}`,
            request: {teamId},
            transformer: (response) => response.approvers
        });
    }

    public getTeams(): Observable<IWebTeamOverview[]> {
        return this.get<IGetTeamsRequest, IGetTeamsResponse, IWebTeamOverview[]>({
            endpoint: `${this.BASEURL}/WebTeam/getTeams`,
            operationId: `getTeams`,
            request: null,
            transformer: (response) => response.teams
        });
    }

    public updateTeam(
        teamId: ITeamId,
        name: string
    ): Observable<IWebTeamOverview> {
        return this.post<IUpdateTeamRequest, IUpdateTeamResponse, IWebTeamOverview>({
            endpoint: `${this.BASEURL}/WebTeam/updateTeam`,
            operationId: `updateTeam`,
            request: {teamId, name},
            transformer: (response) => response.team
        });
    }

}
