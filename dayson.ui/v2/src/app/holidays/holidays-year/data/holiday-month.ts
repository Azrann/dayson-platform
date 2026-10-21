import { HolidayWeek } from './holiday-week';
import { HolidayDay } from './holiday-day';
import { HolidaySet } from './holiday-set';

export class HolidayMonth {
    public days: HolidayDay[];
    public weeks: HolidayWeek[];
    public name: string;

    constructor(
        public month: number,
        public year: number,
    ) {
        this.days = [];
        this.weeks = [];

        this.name = this.getName();

        this.populate();
    }

    public getDay(day: number): HolidayDay {
        return this.days[day - 1];
    }

    private getName() {
        switch (this.month) {
            case 0:
                return 'January';
            case 1:
                return 'February';
            case 2:
                return 'March';
            case 3:
                return 'April';
            case 4:
                return 'May';
            case 5:
                return 'June';
            case 6:
                return 'July';
            case 7:
                return 'August';
            case 8:
                return 'September';
            case 9:
                return 'October';
            case 10:
                return 'November';
            case 11:
                return 'December';
        }

        throw new Error(`Unknown month ${this.month}`);
    }

    private populate() {
        const date = new Date(this.year, this.month, 1);
        let currentWeek = this.addWeek();

        while (date.getMonth() === this.month) {
            const day = currentWeek.addDay(date.getDate());

            this.days.push(day);

            // 0 means sunday -> weeks end on sunday
            if (date.getDay() === 0) {
                currentWeek = this.addWeek();
            }

            date.setDate(date.getDate() + 1);
        }
    }

    private addWeek() {
        const week: HolidayWeek = new HolidayWeek(
            this.weeks.length + 1,
            this.month,
            this.year
        );

        this.weeks.push(week);

        return week;
    }

    public reassignHolidaySets(holidaySets: HolidaySet[]) {
        this.days.forEach((day) => day.reassignHolidaySets(holidaySets));
    }

}
