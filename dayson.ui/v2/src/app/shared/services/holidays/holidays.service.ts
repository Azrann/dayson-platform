import { HolidayStatus } from './data/holiday-status';
import { ComService } from '../infrastructure/com-service';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';
import { Subscriber } from 'rxjs/Subscriber';

import { BaseService } from '../../../shared/services/base-service';
import { UserService } from '../user/user.service';
import { RequestedHoliday } from './data/requested-holiday';

@Injectable()
export class HolidaysService extends ComService {
    constructor(
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public getHolidays(employeeId: string, year: number): Observable<RequestedHoliday[]> {
        return this.get({
            endpoint: `http://dayson-facade.gear.host/v1/holidays/employee/${employeeId}/year/${year}`,
            operationId: `getholidays-${employeeId}-${year}`,
        });
    }

    public addHolidays(
        id: string,
        from: Date,
        to: Date,
        notes: string,
        status: HolidayStatus,
        subscriberId: string
    ): Observable<RequestedHoliday> {
        return this.post({
            endpoint: `http://dayson-facade.gear.host/v1/holidays/add`,
            request: {
                id,
                from,
                to,
                notes,
                status,
                subscriberId
            }
        });
    }
}
