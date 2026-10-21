import { HolidayStatus } from '../../../shared/services/holidays/data/holiday-status';
import { RequestedHoliday } from '../../../shared/services/holidays/data/requested-holiday';
import { HolidayDay } from './holiday-day';
import { HolidayMonth } from './holiday-month';
import { HolidaySet } from './holiday-set';
import { HolidayColor } from './holiday-color';
import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';

export class HolidayYear {
    public months: HolidayMonth[];

    public holidaySets: HolidaySet[] = [];

    constructor(
        public year: number,
    ) {
        this.months = [];

        this.populate();
    }

    private populate() {
        for (let i = 0; i < 12; i++) {
            this.months.push(new HolidayMonth(
                i,
                this.year
            ));
        }
    }

    public getDay(
        day: number,
        month: number
    ): HolidayDay {
        return this.months[month].getDay(day);
    }

    public addHolidayRequests(
        userId: string,
        requestedHolidays: IWebHolidayOverview[],
        useStatusColor: boolean
    ) {
        // console.log('addHolidayRequests:', userId, requestedHolidays, this.year);

        const existingIndex = this.holidaySets.findIndex((hs) => hs.id === userId);
        if (existingIndex >= 0) {
            // console.log(`HolidayYear replacing existing set for userId ${userId}`);
            this.holidaySets[existingIndex].requestedHolidays = requestedHolidays;
        } else {
            let color: HolidayColor = null;
            if (!useStatusColor) {
                color = this.assignNextAvailableColor(requestedHolidays);
            }

            // console.log('color:', color);

            this.holidaySets.push(new HolidaySet(
                userId,
                requestedHolidays,
                color,
            ));

            // console.log(`HolidayYear creating new set for userId ${userId}`);
        }

        this.reassignHolidaySets();
    }

    public removeHolidayRequests(
        userId: string
    ) {
        let i = 0;

        while (i < this.holidaySets.length) {
            if (this.holidaySets[i].id === userId) {
                this.holidaySets.splice(i, 1);
            } else {
                i++;
            }
        }

        this.reassignHolidaySets();
    }

    private assignNextAvailableColor(
        requestedHolidays: IWebHolidayOverview[]
    ): HolidayColor {
        return HolidayColor.backgroundPalette.find((color) => !this.holidaySets.find((hs) => hs.color === color)) ||
            HolidayColor.backgroundPalette[Math.floor(Math.random() * HolidayColor.backgroundPalette.length)];
    }

    private reassignHolidaySets() {
        this.holidaySets.forEach((hs) => {
            hs.resetIndex();
        });

        this.months.forEach((month) => month.reassignHolidaySets(this.holidaySets));
    }

}
