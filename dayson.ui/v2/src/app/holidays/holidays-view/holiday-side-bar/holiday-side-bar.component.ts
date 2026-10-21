import { WebHolidayStatus } from '../../../shared/data/holiday/web-holiday-status';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material';

import { IUserOverview } from '../../../shared/services/user/data/IUserOverview';
import { RequestedHolidayDetailsComponent } from './../requested-holiday-details/requested-holiday-details.component';
import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';
import { IWebUserOverview } from '../../../shared/data/user/web-user-overview';

@Component({
    selector: 'daysonapp-holiday-side-bar',
    templateUrl: './holiday-side-bar.component.html',
    styleUrls: ['./holiday-side-bar.component.css'],
})
export class HolidaySideBarComponent implements OnInit, OnChanges {
    @Input() holidays: IWebHolidayOverview[];
    @Input() year: number;
    @Input() user: IWebUserOverview;
    @Output() updateHolidays = new EventEmitter();
    @Output() navigateToYear = new EventEmitter<number>();
    @Output() holidayClicked = new EventEmitter<IWebHolidayOverview>();

    filteredHolidays: IWebHolidayOverview[];

    constructor(
        private matDialog: MatDialog,
    ) { }

    ngOnInit() {
        this.filteredHolidays = [];

        // console.log('ngOnInit:');
        // console.log(JSON.stringify(this.holidays));
        // console.log(JSON.stringify(this.year));

        this.loadData();
    }

    ngOnChanges(changes: SimpleChanges): void {
        // console.log('ngOnChanges', changes);
        if (changes['holidays']) {
            // changes['holidays'].currentValue)
            this.loadData();
        }
    }

    loadData() {
        this.filterHolidays(this.holidays);
    }

    private filterHolidays(holidays: IWebHolidayOverview[]) {
        // console.log('filtering:');
        // console.log(JSON.stringify(holidays));
        // console.log(JSON.stringify(this.year));
        if (!holidays) {
            this.filteredHolidays = [];
        } else {
            this.filteredHolidays = holidays
                .filter((h) => h.from.year === this.year || h.to.year === this.year)
                .sort((a, b) => a.dateFrom.isBefore(b.dateFrom) ? -1 : 1);
        }
        // console.log('filtered', this.filteredHolidays);
    }

    addNewHoliday() {
        const requestedHoliday = {
            userId: this.user.id,
            companyId: this.user.companyId,
            dateFrom: undefined,
            dateTo: undefined,
            numberOfDays: undefined,
            id: null,
            from: undefined,
            to: undefined,
            notes: '',
            status: WebHolidayStatus.Requested,
        } as IWebHolidayOverview;

        const dialogRef = this.matDialog.open(RequestedHolidayDetailsComponent, {
            width: '700px',
            data: {
                requestedHoliday: requestedHoliday,
                user: this.user,
                year: this.year,
            }
        });

        dialogRef.afterClosed().subscribe((created) => {
            if (created) {
                this.updateHolidays.next();
            }
        });
    }

    navigate(direction: number) {
        this.navigateToYear.next(this.year + direction);
        this.filterHolidays(this.holidays);
    }

    onHolidayClicked(holiday: IWebHolidayOverview) {
        this.holidayClicked.emit(holiday);
    }
}
