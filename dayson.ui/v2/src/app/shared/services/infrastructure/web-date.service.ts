import { Injectable } from '@angular/core';

import { IWebDate } from '../../data/shared/web-date';
import * as moment from 'moment';

@Injectable()
export class WebDateService {
    constructor(
    ) {
        // nothing yet
    }

    public toMoment(od: IWebDate): moment.Moment {
        return moment({
            year: od.year,
            month: od.month - 1,
            date: od.day
        });
    }

    public toWebDate(date: moment.Moment): IWebDate {
        console.log('towebdate: ', date);
        return {
            year: date.year(),
            month: date.month() + 1,
            day: date.date()
        } as IWebDate;
    }

}
