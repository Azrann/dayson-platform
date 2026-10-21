import { IWebHolidayOverview } from '../../shared/data/holiday/web-holiday-overview';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { HolidaysService } from '../../shared/services/holidays/holidays.service';
import { IUserOverview } from '../../shared/services/user/data/IUserOverview';
import { HolidayYear } from '../holidays-year/data/holiday-year';
import { HolidayMonth } from '../holidays-year/data/holiday-month';

@Component({
    selector: 'daysonapp-calendar-year',
    templateUrl: './calendar-year.component.html',
    styleUrls: ['./calendar-year.component.scss']
})
export class CalendarYearComponent {
    @Input() year: HolidayYear;
    @Input() mode: CalendarMode;
    @Output() holidayClicked = new EventEmitter<IWebHolidayOverview>();

    constructor(
        private holidayService: HolidaysService
    ) { }

    monthTracker(index: number, month: HolidayMonth) {
        return month.month;
    }

    onHolidayClicked(holiday: IWebHolidayOverview) {
        this.holidayClicked.emit(holiday);
    }
}

export type CalendarMode = 'Year' | 'Month' | 'Week';
