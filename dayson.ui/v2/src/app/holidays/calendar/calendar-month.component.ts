import { IWebHolidayOverview } from '../../shared/data/holiday/web-holiday-overview';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material';

import { HolidayMonth } from '../holidays-year/data/holiday-month';
import { CalendarMode } from './calendar-year.component';
import { HolidayWeek } from '../holidays-year/data/holiday-week';
import { HolidayDay } from '../holidays-year/data/holiday-day';

@Component({
    selector: 'daysonapp-calendar-month',
    templateUrl: './calendar-month.component.html',
    styleUrls: ['./calendar-month.component.scss']
})
export class CalendarMonthComponent {
    @Input() month: HolidayMonth;
    @Input() mode: CalendarMode;
    @Output() holidayClicked = new EventEmitter<IWebHolidayOverview>();

    constructor(
        private matDialog: MatDialog,
    ) { }

    weekTracker(index: number, week: HolidayWeek) {
        return week.week;
    }

    dayTracker(index: number, day: HolidayDay) {
        return `${index}`;
    }

    onHolidayClicked(holiday: IWebHolidayOverview) {
        this.holidayClicked.emit(holiday);
    }
}
