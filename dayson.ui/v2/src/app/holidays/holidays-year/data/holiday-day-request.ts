import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';
import { HolidayColor } from './holiday-color';
import { HolidaySet } from './holiday-set';
import * as moment from 'moment';

export class HolidayDayRequest {
    holidaySet: HolidaySet;
    holidayRequests: HolidayDayRequest[];

    css: string;
    startColor: HolidayColor;
    middleColor: HolidayColor;
    endColor: HolidayColor;

    startHoliday: IWebHolidayOverview;
    middleHoliday: IWebHolidayOverview;
    endHoliday: IWebHolidayOverview;

    index: number;

    public static getHolidayDayRequest(
        day: Date,
        holidaySet: HolidaySet
    ): HolidayDayRequest {
        const dayMoment = moment(day);
        let isStart = false;
        let isEnd = false;
        let isThrough = false;
        let startColor = null;
        let middleColor = null;
        let endColor = null;
        let index = null;
        let startHoliday: IWebHolidayOverview = null;
        let middleHoliday: IWebHolidayOverview = null;
        let endHoliday: IWebHolidayOverview = null;
        const holidayRequests = [];

        holidaySet.requestedHolidays.forEach((webHolidayOverview) => {
            const requestStart = dayMoment.isSame(webHolidayOverview.dateFrom);
            const requestEnd = dayMoment.isSame(webHolidayOverview.dateTo);
            const requestThrough =
                dayMoment.isAfter(webHolidayOverview.dateFrom) &&
                dayMoment.isBefore(webHolidayOverview.dateTo);

            if (requestStart) {
                isStart = true;

                if (holidaySet.color) {
                    startColor = holidaySet.color;
                } else {
                    startColor = HolidayColor.getByStatus(webHolidayOverview.status);
                }

                index = index || webHolidayOverview.index;
                holidayRequests.push(webHolidayOverview);
                startHoliday = webHolidayOverview;
            }

            if (requestEnd) {
                isEnd = true;

                if (holidaySet.color) {
                    endColor = holidaySet.color;
                } else {
                    endColor = HolidayColor.getByStatus(webHolidayOverview.status);
                }

                index = index || webHolidayOverview.index;
                holidayRequests.push(webHolidayOverview);
                endHoliday = webHolidayOverview;
            }

            if (requestThrough) {
                isThrough = true;

                if (holidaySet.color) {
                    middleColor = holidaySet.color;
                } else {
                    middleColor = HolidayColor.getByStatus(webHolidayOverview.status);
                }

                index = index || webHolidayOverview.index;
                holidayRequests.push(webHolidayOverview);
                middleHoliday = webHolidayOverview;
            }
        });

        if (!isStart && !isEnd && !isThrough) {
            return null;
        }

        const result = new HolidayDayRequest();
        result.holidaySet = holidaySet;
        result.startColor = startColor;
        result.endColor = endColor;
        result.middleColor = middleColor;
        result.css = 'holiday-request';
        result.holidayRequests = holidayRequests;
        result.startHoliday = startHoliday;
        result.middleHoliday = middleHoliday;
        result.endHoliday = endHoliday;

        result.setIndex(index || null);

        return result;
    }

    public setIndex(index: number) {
        // console.log(`Setting index of ${this.holidaySet.id} to ${index}`);
        this.holidayRequests.forEach((hr) => {
            hr.index = index;
        });

        this.index = index;
    }
}
