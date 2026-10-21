import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';
import { RequestedHoliday } from '../../../shared/services/holidays/data/requested-holiday';
import { HolidayColor } from './holiday-color';

export class HolidaySet {
    constructor(
        public id: string,
        public requestedHolidays: IWebHolidayOverview[],
        public color: HolidayColor,
    ) {
    }

    public resetIndex() {
        this.requestedHolidays.forEach((rh) => {
            rh.index = null;
        });
    }
}
