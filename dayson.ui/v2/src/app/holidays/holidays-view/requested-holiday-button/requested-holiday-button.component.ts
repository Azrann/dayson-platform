import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material';

import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';
import { IWebUserOverview } from '../../../shared/data/user/web-user-overview';
import { WebDateService } from '../../../shared/services/infrastructure/web-date.service';
import { RequestedHolidayDetailsComponent } from './../requested-holiday-details/requested-holiday-details.component';

@Component({
    selector: 'daysonapp-requested-holiday-button',
    templateUrl: './requested-holiday-button.component.html',
    styleUrls: ['./requested-holiday-button.component.scss']
})
export class RequestedHolidayButtonComponent implements OnInit, OnChanges {
    @Input() holiday: IWebHolidayOverview;
    @Input() user: IWebUserOverview;
    @Output() holidayClicked = new EventEmitter<IWebHolidayOverview>();

    dateText: string;

    constructor(
        private webDateService: WebDateService
    ) { }

    ngOnInit() {
    }

    ngOnChanges(change: SimpleChanges) {
        const fromDate = this.holiday.from;
        const to = this.holiday.to;

        const fromDay = this.holiday.from.day;
        const toDay = this.holiday.to.day;

        const fromMonth = this.getMonthName(this.holiday.from.month);
        const toMonth = this.getMonthName(this.holiday.to.month);

        if (fromMonth !== toMonth) {
            this.dateText = `${fromDay} ${fromMonth} - ${toDay} ${toMonth}`;
        } else {
            this.dateText = `${fromDay} - ${toDay} ${toMonth}`;
        }
    }

    openHoliday() {
        this.holidayClicked.emit(this.holiday);
    }

    private getMonthName(month: number): string {
        switch (month) {
            case 1: return 'jan';
            case 2: return 'feb';
            case 3: return 'mar';
            case 4: return 'apr';
            case 5: return 'may';
            case 6: return 'jun';
            case 7: return 'jul';
            case 8: return 'aug';
            case 9: return 'sep';
            case 10: return 'oct';
            case 11: return 'nov';
            case 12: return 'dec';
        }
    }
}
