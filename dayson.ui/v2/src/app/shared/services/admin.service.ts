import { httpFactory } from '@angular/http/src/http_module';
import { AuthHttp } from 'angular2-jwt';
import { BaseService } from '../../shared/services/base-service';
import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';


@Injectable()
export class AdminService extends BaseService {
    constructor(
        http: Http,
        authHttp: AuthHttp
    ) {
        super(http, authHttp);
    }

    public adminAddHolidays(
        from: Date,
        to: Date,
        employeeId: string,
        notes: string
    ): Observable<string> {
        return this.post<string>('admin/holidays', <IAdminAddHolidaysRequest>{
            from,
            to,
            employeeId,
            notes
        });
    }

    public adminCancelHolidays(
        holidayId: string,
    ): Observable<any> {
        return this.post<any>('admin/holidays/cancel', <IAdminCancelHolidaysRequest>{
            holidayId,
        });
    }

    public adminRejectHolidays(
        holidayId: string
    ): Observable<any> {
        return this.post<any>('admin/holidays/reject', <IAdminRejectHolidaysRequest>{
            holidayId,
        });
    }

    public adminApproveHolidays(
        holidayId: string
    ): Observable<any> {
        return this.post<any>('admin/holidays/approve', <IAdminApproveHolidaysRequest>{
            holidayId,
        });
    }
}

export interface IAdminApproveHolidaysRequest {
    holidayId: string;
}

export interface IAdminRejectHolidaysRequest {
    holidayId: string;
}

export interface IAdminCancelHolidaysRequest {
    holidayId: string;
}

export interface IAdminAddHolidaysRequest {
    from: Date;
    to: Date;
    employeeId: string;
    notes: string;
}

