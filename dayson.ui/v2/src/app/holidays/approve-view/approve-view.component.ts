import { WebTeamService } from '../../shared/services/web-team/web-team.service';
import { WebHolidayService } from '../../shared/services/web-holiday/web-holiday.service';
import { IWebTeamOverview } from '../../shared/data/team/web-team-overview';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { NavigationBarService } from '../../shared/navigation-bar/navigration-bar.service';
import { IWebHolidayOverview } from '../../shared/data/holiday/web-holiday-overview';
import { IWebUserOverview } from '../../shared/data/user/web-user-overview';
import { WebUserService } from '../../shared/services/web-user/web-user.service';
import { IWebDate } from '../../shared/data/shared/web-date';
import { WebHolidayStatus } from '../../shared/data/holiday/web-holiday-status';

@Component({
    selector: 'daysonapp-approve-view',
    templateUrl: './approve-view.component.html',
    styleUrls: ['./approve-view.component.scss']
})
export class ApproveViewComponent implements OnInit {
    teams: IWebTeamOverview[];
    users: IWebUserOverview[];
    holidays: IWebHolidayOverview[];

    usersById: { [userId: string]: IWebUserOverview };

    loading: boolean;

    constructor(
        private webHolidayService: WebHolidayService,
        private webUserService: WebUserService,
        private webTeamService: WebTeamService,
        private navigationBarService: NavigationBarService,
    ) { }

    ngOnInit() {
        this.loading = true;

        this.webTeamService.getTeams().subscribe((teams: IWebTeamOverview[]) => {
            this.teams = teams;
            this.generateData();
        });

        this.webUserService.searchUsers(null, [], false).subscribe((users: IWebUserOverview[]) => {
            this.users = users;
            this.generateData();
        });

        console.log('Holidays locked to 2018 for now');
        this.webHolidayService.searchHolidays(
            null,
            null,
            { year: 2018, month: 1, day: 1 } as IWebDate,
            { year: 2018, month: 12, day: 31 } as IWebDate,
            WebHolidayStatus.Requested
        ).subscribe((holidays: IWebHolidayOverview[]) => {
            this.holidays = holidays;
            this.generateData();
        });

        this.navigationBarService.setNavMenu(null, 'holidays-approve-view');
    }

    private generateData() {
        if (!this.teams || !this.users || !this.holidays) {
            return;
        }

        this.loading = false;

        this.usersById = {};
        this.users.forEach((user: IWebUserOverview) => {
            this.usersById[user.id as any] = user;
        });
    }

    reject(holiday: IWebHolidayOverview) {
        console.log('holiday.id', holiday.id);
        this.webHolidayService.changeHolidayStatus(
            holiday.id,
            WebHolidayStatus.Rejected
        ).subscribe((updatedHoliday) => {
            this.holidays.splice(
                this.holidays.indexOf(holiday),
                1
            );
        });
    }

    approve(holiday: IWebHolidayOverview) {
        this.webHolidayService.changeHolidayStatus(
            holiday.id,
            WebHolidayStatus.Accepted
        ).subscribe((updatedHoliday) => {
            this.holidays.splice(
                this.holidays.indexOf(holiday),
                1
            );
        });
    }
}
