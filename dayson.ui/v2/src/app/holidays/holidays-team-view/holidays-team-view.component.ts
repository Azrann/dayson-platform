import { HolidayColor } from '../holidays-year/data/holiday-color';
import { Component, OnInit } from '@angular/core';

import { IWebHolidayOverview } from '../../shared/data/holiday/web-holiday-overview';
import { IWebUserOverview } from '../../shared/data/user/web-user-overview';
import { NavigationBarService } from '../../shared/navigation-bar/navigration-bar.service';
import { WebHolidayService } from '../../shared/services/web-holiday/web-holiday.service';
import { CalendarMode } from '../calendar/calendar-year.component';
import { HolidayYear } from '../holidays-year/data/holiday-year';
import { MatSelectChange } from '@angular/material';

@Component({
    selector: 'daysonapp-holidays-team-view',
    templateUrl: './holidays-team-view.component.html',
    styleUrls: ['./holidays-team-view.component.scss']
})
export class HolidaysTeamViewComponent implements OnInit {
    holidayYear: HolidayYear;

    year: number;
    mode: CalendarMode = 'Year';

    userColors:  { [userId: string]: string } = {};

    constructor(
        private navigationBarService: NavigationBarService,
        private webHolidayService: WebHolidayService,
    ) { }

    ngOnInit() {
        this.navigationBarService.setNavMenu(null, 'holidays-team-view');

        this.year = 2018;
        this.holidayYear = new HolidayYear(this.year);

        this.mode = localStorage.getItem('calendarMode') as any || 'Year';
    }

    modeChanged($event: MatSelectChange) {
        localStorage.setItem('calendarMode', $event.value);
    }

    addUser(user: IWebUserOverview) {
        this.webHolidayService.getUserHolidays(user.id).subscribe((holidays: IWebHolidayOverview[]) => {
            this.holidayYear.addHolidayRequests(
                user.id as any,
                holidays,
                false
            );

            this.updateUserColors();
        });
    }

    removeUser(user: IWebUserOverview) {
        this.holidayYear.removeHolidayRequests(
            user.id as any,
        );
    }

    refreshUser(user: IWebUserOverview) {
        this.webHolidayService.getUserHolidays(user.id).subscribe((holidays: IWebHolidayOverview[]) => {
            this.holidayYear.addHolidayRequests(
                user.id as any,
                holidays,
                false
            );

            this.updateUserColors();
        });
    }

    private updateUserColors() {
        this.userColors = {};
        this.holidayYear.holidaySets.forEach((hs) => {
            this.userColors[hs.id] = HolidayColor.toForegroundColor(hs.color) as any;
        });
    }
}
