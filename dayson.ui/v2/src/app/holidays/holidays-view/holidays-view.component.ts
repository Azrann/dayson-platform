import { RequestedHolidayDetailsComponent } from './requested-holiday-details/requested-holiday-details.component';
import { IWebDate } from '../../shared/data/shared/web-date';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ISubscription } from 'rxjs/Subscription';

import { IUserId } from '../../shared/data/user/user-id';
import { IWebUserDetails } from '../../shared/data/user/web-user-details';
import { RequestedHoliday } from '../../shared/services/holidays/data/requested-holiday';
import { WebHolidayService } from '../../shared/services/web-holiday/web-holiday.service';
import { WebUserService } from '../../shared/services/web-user/web-user.service';
import { CalendarMode } from '../calendar/calendar-year.component';
import { HolidayYear } from '../holidays-year/data/holiday-year';
import { NavigationBarService } from './../../shared/navigation-bar/navigration-bar.service';
import { IWebHolidayOverview } from '../../shared/data/holiday/web-holiday-overview';
import { MatDialog, MatSelectChange } from '@angular/material';

@Component({
    selector: 'daysonapp-holidays-view',
    templateUrl: './holidays-view.component.html',
    styleUrls: ['./holidays-view.component.css'],
})
export class HolidaysViewComponent implements OnInit, OnDestroy {
    year: number;
    mode: CalendarMode;
    userId: IUserId;
    user: IWebUserDetails;

    holidays: IWebHolidayOverview[];

    holidayYear: HolidayYear;

    loading: boolean;

    updateHolidaysSubscription: ISubscription;

    constructor(
        private navigationBarService: NavigationBarService,
        private activatedRoute: ActivatedRoute,
        private webUserService: WebUserService,
        private webHolidayService: WebHolidayService,
        private router: Router,
        private matDialog: MatDialog,
    ) { }

    ngOnInit() {
        console.log('oninit');
        this.loading = true;
        this.mode = 'Year';
        this.year = new Date().getFullYear();

        this.activatedRoute.params.subscribe((params) => {
            this.loading = true;
            this.year = Number.parseInt(params['year']);

            if (!this.year) {
                throw new Error(`Couldnt get proper year from params: ${params['year']}`);
            }

            if (this.year < 2018) {
                this.year = 2018;
            }
            if (this.year > 2019) {
                this.year = 2019;
            }

            this.userId = params['userId'];
            if (!this.user) {
                if (!this.userId || this.userId as any === '') {
                    this.webUserService.getCurrentUserDetails().subscribe((user: IWebUserDetails) => {
                        this.user = user;
                        this.setUpMenu();
                        this.updateHolidays();
                    });
                } else {
                    this.webUserService.getUserDetails(this.userId).subscribe((user: IWebUserDetails) => {
                        this.user = user;
                        this.setUpMenu();
                        this.updateHolidays();
                    });
                }
            } else {
                this.updateHolidays();
            }
        });
        this.mode = localStorage.getItem('calendarMode') as any || 'Year';
    }

    modeChanged($event: MatSelectChange) {
        localStorage.setItem('calendarMode', $event.value);
    }

    setUpMenu(): void {
        this.navigationBarService.setNavMenu(null, 'holidays-view');
    }

    ngOnDestroy() {
        this.updateHolidaysSubscription.unsubscribe();
    }

    goToYear(year: number) {
        if (!this.userId) {
            this.router.navigateByUrl(`/holidays/view/year/${year}`);
        } else {
            this.router.navigateByUrl(`/holidays/view/user/${this.userId}/year/${year}`);
        }

        this.year = year;

        // this.updateHolidays();
    }

    updateHolidays() {
        this.updateHolidaysSubscription = this.webHolidayService.getUserHolidays(this.user.id).subscribe((holidays) => {
            // console.log('update holidays done');

            this.loading = false;

            // console.log('received holidays in holidaysview:', requestedHolidays);
            this.holidays = holidays;

            this.holidayYear = new HolidayYear(this.year);

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h1',
            //     employeeId: this.userId,
            //     from: new Date(2018, 1, 7).toISOString(),
            //     to: new Date(2018, 1, 9).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Accepted,
            //     numberOfDays: 3,
            //     notes: 'winter holidays acc',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h2',
            //     employeeId: this.userId,
            //     from: new Date(2018, 1, 9).toISOString(),
            //     to: new Date(2018, 1, 14).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Requested,
            //     numberOfDays: 5,
            //     notes: 'winter holidays req',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h3',
            //     employeeId: this.userId,
            //     from: new Date(2018, 1, 14).toISOString(),
            //     to: new Date(2018, 1, 19).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Rejected,
            //     numberOfDays: 5,
            //     notes: 'winter holidays',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h4',
            //     employeeId: this.userId,
            //     from: new Date(2018, 1, 19).toISOString(),
            //     to: new Date(2018, 1, 24).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Canceled,
            //     numberOfDays: 5,
            //     notes: 'winter holidays can',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h5',
            //     employeeId: this.userId,
            //     from: new Date(2018, 1, 18).toISOString(),
            //     to: new Date(2018, 1, 27).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Accepted,
            //     numberOfDays: 5,
            //     notes: 'winter holidays can',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h6',
            //     employeeId: this.userId,
            //     from: new Date(2018, 1, 23).toISOString(),
            //     to: new Date(2018, 1, 25).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Rejected,
            //     numberOfDays: 5,
            //     notes: 'winter holidays can',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h7',
            //     employeeId: this.userId,
            //     from: new Date(2018, 2, 10).toISOString(),
            //     to: new Date(2018, 2, 11).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Requested,
            //     numberOfDays: 5,
            //     notes: 'winter holidays can',
            //     year: 2018
            // } as IRequestedHolidayDto));

            // this.requestedHolidays.push(RequestedHoliday.fromDto({
            //     id: 'h8',
            //     employeeId: this.userId,
            //     from: new Date(2018, 2, 9).toISOString(),
            //     to: new Date(2018, 2, 10).toISOString(),
            //     createdOn: new Date(2018, 0, 1).toISOString(),
            //     lastUpdated: new Date(2018, 0, 2).toISOString(),
            //     status: HolidayStatus.Rejected,
            //     numberOfDays: 5,
            //     notes: 'winter holidays can',
            //     year: 2018
            // } as IRequestedHolidayDto));

            this.holidays.forEach((rh) => {
                this.holidayYear.addHolidayRequests(rh.id as any, [rh], true);
            });

            console.log('this.holidays:', this.holidays);
            console.log('this.holidayYear:', this.holidayYear);
        });
    }

    openHolidayDetails(holiday: IWebHolidayOverview) {
        const dialogRef = this.matDialog.open(RequestedHolidayDetailsComponent, {
            width: '700px',
            data: {
                requestedHoliday: holiday,
                user: this.user,
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });
    }
}
