import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';

import { IUserOverview } from '../../../shared/services/user/data/IUserOverview';
import { RequestedHolidayDetailsComponent } from '../../holidays-view/requested-holiday-details/requested-holiday-details.component';
import { HolidayDay } from '../data/holiday-day';
import { HolidayMonth } from '../data/holiday-month';

@Component({
    selector: 'daysonapp-holidays-month',
    templateUrl: './holidays-month.component.html',
    styleUrls: ['./holidays-month.component.scss']
})
export class HolidaysMonthComponent implements OnInit {
    @Input() holidayMonth: HolidayMonth;

    constructor(
        private matDialog: MatDialog,
    ) { }

    ngOnInit() {
    }

    hoverDay(day: HolidayDay, hovered: boolean) {
        // if (day.requestedHolidays && day.requestedHolidays.length > 0) {
        //     day.requestedHolidays[0].hovered = hovered;
        // }
    }

    dayClicked(day: HolidayDay) {
        console.log('day clicked:', day);
        const dialogRef = this.matDialog.open(RequestedHolidayDetailsComponent, {
            width: '700px',
            data: {
                // requestedHoliday: day.requestedHolidays[0],
                // user: day.requestedHolidays[0].user,
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });
    }
}
