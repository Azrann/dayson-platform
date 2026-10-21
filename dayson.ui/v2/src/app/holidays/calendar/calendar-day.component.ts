import { HolidayDayRequest } from '../holidays-year/data/holiday-day-request';
import { Component, Input, EventEmitter, Output } from '@angular/core';
import { MatDialog } from '@angular/material';

import { HolidayDay } from '../holidays-year/data/holiday-day';
import { IWebHolidayOverview } from '../../shared/data/holiday/web-holiday-overview';

@Component({
    selector: 'daysonapp-calendar-day',
    templateUrl: './calendar-day.component.html',
    styleUrls: ['./calendar-day.component.scss']
})
export class CalendarDayComponent {
    @Input() day: HolidayDay;
    @Output() holidayClicked = new EventEmitter<IWebHolidayOverview>();

    constructor(
    ) { }

    startClicked(holidayDayRequest: HolidayDayRequest) {
        this.holidayClicked.emit(holidayDayRequest.startHoliday);
    }

    endClicked(holidayDayRequest: HolidayDayRequest) {
        this.holidayClicked.emit(holidayDayRequest.endHoliday);
    }

    middleClicked(holidayDayRequest: HolidayDayRequest) {
        this.holidayClicked.emit(holidayDayRequest.middleHoliday);
    }
}
