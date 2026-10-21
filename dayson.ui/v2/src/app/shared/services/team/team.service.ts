import { Observable } from 'rxjs/Observable';
import { AuthHttp } from 'angular2-jwt';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { BaseService } from '../base-service';
import { ComService } from '../infrastructure/com-service';
import { ITeam } from '../user/data/ITeam';


@Injectable()
export class TeamService extends ComService {

    constructor(
        http: Http,
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public getTeamBasicInfo(
        id: string
    ): Observable<ITeam> {
        return this.get({
            endpoint: `http://dayson-facade.gear.host/v1/company/team/${id}`,
        });
    }

    public createTeam(
        name: string
    ): Observable<string> {
        console.log('creating team with name ', name);

        return this.post({
            endpoint: `http://dayson-facade.gear.host/v1/company/team`,
            request: { name }
        });
    }

    public updateTeam(
        teamId: string,
        name: string
    ): Observable<any> {
        throw new Error('not implemented');
    }
}
