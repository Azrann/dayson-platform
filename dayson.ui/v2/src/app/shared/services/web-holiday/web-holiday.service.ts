import { Injectable } from '@angular/core';
import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';

import { IHolidayId } from '../../data/holiday/holiday-id';
import { IWebHolidayOverview } from '../../data/holiday/web-holiday-overview';
import { WebHolidayStatus } from '../../data/holiday/web-holiday-status';
import { IWebDate } from '../../data/shared/web-date';
import { ITeamId } from '../../data/team/team-id';
import { IUserId } from '../../data/user/user-id';
import { ComService } from '../infrastructure/com-service';
import { WebDateService } from '../infrastructure/web-date.service';
import { IChangeHolidayStatusRequest, IChangeHolidayStatusResponse } from './dtos/change-holiday-status';
import { ICreateHolidayRequest, ICreateHolidayResponse } from './dtos/create-holiday';
import { IGetUserHolidaysRequest, IGetUserHolidaysResponse } from './dtos/get-user-holidays';
import { ISearchHolidaysRequest, ISearchHolidaysResponse } from './dtos/search-holidays';
import { IUpdateHolidayRequest, IUpdateHolidayResponse } from './dtos/update-holiday';

@Injectable()
export class WebHolidayService extends ComService {
    constructor(
        authHttp: AuthHttp,
        private webDateService: WebDateService,
    ) {
        super(authHttp);
    }

    public getUserHolidays(
        userId: IUserId,
    ): Observable<IWebHolidayOverview[]> {
        console.log('userid is', userId);

        return this.get<IGetUserHolidaysRequest, IGetUserHolidaysResponse, IWebHolidayOverview[]>({
            endpoint: `${this.BASEURL}/WebHoliday/getUserHolidays`,
            operationId: `getUserHolidays`,
            request: { userId },
            transformer: (response) => {
                response.holidays.forEach((rh) => {
                    rh.dateFrom = this.webDateService.toMoment(rh.from);
                    rh.dateTo = this.webDateService.toMoment(rh.to);
                });

                return response.holidays;
            }
        });
    }

    public createHoliday(
        userId: IUserId,
        notes: string,
        from: IWebDate,
        to: IWebDate
    ): Observable<IWebHolidayOverview> {
        return this.post<ICreateHolidayRequest, ICreateHolidayResponse, IWebHolidayOverview>({
            endpoint: `${this.BASEURL}/WebHoliday/createHoliday`,
            operationId: `createHoliday`,
            request: { userId, notes, from, to },
        });
    }

    public updateHoliday(
        holidayId: IHolidayId,
        notes: string,
        from: IWebDate,
        to: IWebDate
    ): Observable<IWebHolidayOverview> {
        return this.post<IUpdateHolidayRequest, IUpdateHolidayResponse, IWebHolidayOverview>({
            endpoint: `${this.BASEURL}/WebHoliday/updateHoliday`,
            operationId: `updateHoliday`,
            request: { holidayId, notes, from, to },
            transformer: (response) => {
                response.holiday.dateFrom = this.webDateService.toMoment(response.holiday.from);
                response.holiday.dateTo = this.webDateService.toMoment(response.holiday.to);

                return response.holiday;
            }
        });
    }

    public changeHolidayStatus(
        holidayId: IHolidayId,
        status: WebHolidayStatus
    ): Observable<IWebHolidayOverview> {
        return this.post<IChangeHolidayStatusRequest, IChangeHolidayStatusResponse, IWebHolidayOverview>({
            endpoint: `${this.BASEURL}/WebHoliday/changeHolidayStatus`,
            operationId: `changeHolidayStatus`,
            request: { holidayId, status },
            transformer: (response) => {
                response.holiday.dateFrom = this.webDateService.toMoment(response.holiday.from);
                response.holiday.dateTo = this.webDateService.toMoment(response.holiday.to);

                return response.holiday;
            }
        });
    }

    public searchHolidays(
        teamIds: ITeamId[],
        userIds: IUserId[],
        from: IWebDate,
        to: IWebDate,
        status: WebHolidayStatus,
    ): Observable<IWebHolidayOverview[]> {
        return this.post<ISearchHolidaysRequest, ISearchHolidaysResponse, IWebHolidayOverview[]>({
            endpoint: `${this.BASEURL}/WebHoliday/searchHolidays`,
            operationId: `searchHolidays`,
            request: { teamIds, userIds, from, to, status },
            transformer: (response) => {
                response.holidays.forEach((rh) => {
                    rh.dateFrom = this.webDateService.toMoment(rh.from);
                    rh.dateTo = this.webDateService.toMoment(rh.to);
                });

                return response.holidays;
            }
        });
    }

}
