import { HolidayDay } from './holiday-day';

export class HolidayWeek {
    public weekNumber: number;

    public days: HolidayDay[];

    constructor(
        public week: number,
        public month: number,
        public year: number
    ) {
        this.days = new Array(7);
    }

    public addDay(day: number): HolidayDay {
        const holidayDay = new HolidayDay(
            day,
            this.month,
            this.year
        );

        // console.log('adding day', holidayDay);

        this.days[holidayDay.dayOfWeek - 1] = holidayDay;

        if (!this.weekNumber) {
            this.setWeekNumber();
        }

        return holidayDay;
    }

    private setWeekNumber = function () {
        // Copy date so don't modify original
        const d = new Date(Date.UTC(this.year, this.month - 1, this.day));
        // Set to nearest Thursday: current date + 4 - current day number
        // Make Sunday's day number 7
        d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));
        // Get first day of year
        const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1)).getTime();
        // Calculate full weeks to nearest Thursday
        const weekNo = Math.ceil((((d.getTime() - yearStart) / 86400000) + 1) / 7);

        this.weekNumber = weekNo;
    };
}
