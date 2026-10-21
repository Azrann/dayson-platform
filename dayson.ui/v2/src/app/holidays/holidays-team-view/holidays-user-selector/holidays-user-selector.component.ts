import {
    RequestedHolidayDetailsComponent,
} from '../../holidays-view/requested-holiday-details/requested-holiday-details.component';
import { WebHolidayStatus } from '../../../shared/data/holiday/web-holiday-status';
import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';
import { IWebTeamOverview } from '../../../shared/data/team/web-team-overview';
import { WebTeamService } from '../../../shared/services/web-team/web-team.service';
import { WebHolidayService } from '../../../shared/services/web-holiday/web-holiday.service';
import { WebUserService } from '../../../shared/services/web-user/web-user.service';
import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';

import { HolidayStatus } from '../../../shared/services/holidays/data/holiday-status';
import { IWebUserOverview } from '../../../shared/data/user/web-user-overview';
import { ITeamId } from '../../../shared/data/team/team-id';
import { MatDialog } from '@angular/material';

@Component({
    selector: 'daysonapp-holidays-user-selector',
    templateUrl: './holidays-user-selector.component.html',
    styleUrls: ['./holidays-user-selector.component.scss']
})
export class HolidaysUserSelectorComponent implements OnInit {
    @Output() addUser = new EventEmitter<IWebUserOverview>();
    @Output() removeUser = new EventEmitter<IWebUserOverview>();
    @Output() refreshUser = new EventEmitter<IWebUserOverview>();
    @Input() year: number;
    @Input() userColors:  { [userId: string]: string };

    selectedDict: { [userId: string]: boolean };

    users: IWebUserOverview[];
    teams: IWebTeamOverview[];

    teamGroups: ITeamGroup[];
    teamGroupDictionary: { [teamId: string]: ITeamGroup };

    loading: boolean;

    constructor(
        private matDialog: MatDialog,
        private webUserService: WebUserService,
        private webHolidayService: WebHolidayService,
        private webTeamService: WebTeamService,
    ) { }

    ngOnInit() {
        this.loading = true;

        this.teamGroups = [];
        this.teamGroupDictionary = {};
        this.selectedDict = {};

        this.webTeamService.getTeams().subscribe((teams) => {
            this.teams = teams;

            teams.forEach((team) => {
                let teamGroup = this.teamGroupDictionary[team.id as any];

                if (!teamGroup) {
                    teamGroup = {
                        team: {},
                        users: []
                    } as ITeamGroup;

                    this.teamGroupDictionary[team.id as any] = teamGroup;
                    this.teamGroups.push(teamGroup);
                }

                teamGroup.team = team;
            });

            this.updateLoading();
        });

        this.webUserService.searchUsers('', [], true).subscribe((users) => {
            this.users = users;

            users.forEach((user) => {
                let teamGroup = this.teamGroupDictionary[user.teamId as any];

                if (!teamGroup) {
                    teamGroup = {
                        team: {},
                        users: []
                    } as ITeamGroup;

                    this.teamGroupDictionary[user.teamId as any] = teamGroup;
                    this.teamGroups.push(teamGroup);
                }

                teamGroup.users.push(user);
            });

            this.updateLoading();
        });
    }

    updateLoading() {
        this.loading = !this.users || !this.teams;
    }

    switchUser(user: IWebUserOverview) {
        // we don't support selecting, unselecting
        // just one user at a time
        if (this.selectedDict[user.id as any]) {
            delete this.selectedDict[user.id as any];
            this.removeUser.emit(user);
        } else {
            this.selectedDict[user.id as any] = true;

            this.addUser.emit(user);
        }
    }

    addHoliday($event: MouseEvent, user: IWebUserOverview) {
        console.log('addHoliday clicked', $event, user);
        event.stopPropagation();

        const requestedHoliday = {
            userId: user.id,
            companyId: user.companyId,
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
                user: user,
                year: this.year,
            }
        });

        dialogRef.afterClosed().subscribe((created) => {
            if (created) {
                if (this.selectedDict[user.id as any]) {
                    this.refreshUser.emit(user);
                } else {
                    this.switchUser(user);
                }
            }
        });

    }
}

interface ITeamGroup {
    team: IWebTeamOverview;
    users: IWebUserOverview[];
}
