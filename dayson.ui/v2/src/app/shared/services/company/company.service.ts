import { ComService } from '../infrastructure/com-service';
import { IGlobalCompanyRules } from './data/IGlobalCompanyRules';
import { ICompany } from './data/ICompany';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { AuthHttp } from 'angular2-jwt';

import { BaseService } from '../base-service';
import { ICreateEmployeeRequest } from './dtos/CreateEmployeeDtos';
import { ITeamBasicInfo } from './dtos/GetTeamsDtos';
import { Subscriber } from 'rxjs/Subscriber';

@Injectable()
export class CompanyService extends ComService {
    constructor(
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public ggetGlobalRules(): Observable<IGlobalCompanyRules> {
        return this.get({
            endpoint: 'http://dayson-facade.gear.host/v1/globalrules',
            useCaching: true,
        });
    }

    public ssetGlobalRules(rules: IGlobalCompanyRules): Observable<IGlobalCompanyRules> {
        return this.post({
            endpoint: 'http://dayson-facade.gear.host/v1/globalrules',
            request: rules
        });
    }

    public createEmployee(emailAddress: string, name: string): Observable<string> {
        return this.post({
            endpoint: 'http://dayson-facade.gear.host/v1/company/employee',
            request: <ICreateEmployeeRequest>{
                emailAddress,
                name
            }
        });
    }

    public updateCompany(company: ICompany): Observable<any> {
        return new Observable((subscriber: Subscriber<ICompany>) => {
            Observable.timer(300).subscribe(() => {
                // we don't do anything because we are using a reference for this hacked version
                subscriber.next();
            });
        });
    }

    public teamAddEmployee(employeeId: string, teamId: string): Observable<any> {
        return this.post({
            endpoint: 'http://dayson-facade.gear.host/v1/company/team/addEmployee',
            request: {
                employeeId,
                teamId
            }
        });
    }

    public getTeams(): Observable<ITeamBasicInfo[]> {
        return this.get({
            endpoint: 'http://dayson-facade.gear.host/v1/company/teams',
            useCaching: true,
        });
    }
}

