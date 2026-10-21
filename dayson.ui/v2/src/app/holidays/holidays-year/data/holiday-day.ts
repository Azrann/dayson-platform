import { HolidayStatus } from '../../../shared/services/holidays/data/holiday-status';
import { RequestedHoliday } from '../../../shared/services/holidays/data/requested-holiday';
import { HolidaySet } from './holiday-set';
import { HolidayDayRequest } from './holiday-day-request';

export class HolidayDay {
    public dayOfWeek: number;

    public holidayDayRequests: HolidayDayRequest[];

    constructor(
        public day: number,
        public month: number,
        public year: number,
    ) {
        this.dayOfWeek = new Date(
            year,
            month,
            day
        ).getDay();

        // console.log(`day of week for ${day}/${month}/${year} is ${this.dayOfWeek}`);

        if (this.dayOfWeek === 0) {
            this.dayOfWeek = 7;
        }

        this.holidayDayRequests = [];
    }

    public reassignHolidaySets(holidaySets: HolidaySet[]) {
        const today = new Date(this.year, this.month, this.day);
        this.holidayDayRequests = new Array();

        const newDayRequests: HolidayDayRequest[] = [];

        for (let i = 0; i < holidaySets.length; i++) {
            const dayRequest: HolidayDayRequest = HolidayDayRequest.getHolidayDayRequest(today, holidaySets[i]);

            if (dayRequest) {
                // console.log(`Found day request ${dayRequest.holidaySet.id} for day ${this.day}.${this.month}`, dayRequest);

                if (dayRequest.index !== null) {
                    // console.log(`Adding holidayset ${dayRequest.holidaySet.id} to day ${this.day}.${this.month} at index ${dayRequest.index}`);
                    this.holidayDayRequests[dayRequest.index - 1] = dayRequest;
                } else {
                    // console.log(`Found new holidayset ${dayRequest.holidaySet.id} to day ${this.day}.${this.month}`, dayRequest);
                    newDayRequests.push(dayRequest);
                }
            }
        }

        newDayRequests.forEach((dr) => {
            const index = this.getNextEmptySpace();

            dr.setIndex(index);
            this.holidayDayRequests[dr.index - 1] = dr;
            // console.log(`Adding new holidayset ${dr.holidaySet.id} to day ${this.day}.${this.month} at index ${index}`);
        });
    }

    private getNextEmptySpace() {
        let i = 0;

        while (i < this.holidayDayRequests.length && this.holidayDayRequests[i]) {
            i++;
        }

        return i + 1;
    }
}
