import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { ITeamId } from '../../shared/data/team/team-id';
import { IWebTeamOverview } from '../../shared/data/team/web-team-overview';
import { IWebUserDetails } from '../../shared/data/user/web-user-details';
import { IWebUserOverview } from '../../shared/data/user/web-user-overview';
import { NavigationBarService } from '../../shared/navigation-bar/navigration-bar.service';
import { IUserBasicInfo } from '../../shared/services/auth-user/data/AuthUser';
import { WebTeamService } from '../../shared/services/web-team/web-team.service';
import { WebUserService } from '../../shared/services/web-user/web-user.service';
import { ITeamOverview } from './../../shared/services/user/data/ITeamOverview';
import { AdminTeamEditComponent } from './admin-team-edit/admin-team-edit.component';
import { AdminTeamUserEditComponent } from './admin-team-user-edit/admin-team-user-edit.component';

@Component({
    selector: 'daysonapp-admin-team-view',
    templateUrl: './admin-team-view.component.html',
    styleUrls: ['./admin-team-view.component.scss']
})
export class AdminTeamViewComponent implements OnInit {
    private searchTimeout: any = null;

    teams: IWebTeamOverview[];
    teamDict: { [id: string]: IWebTeamOverview };
    users: IWebUserOverview[];

    searchText: string;

    selectedTeamIds: ITeamId[] = [];
    selectedTeamIdDict: any = {};

    loading: boolean;
    loadingUsers: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private matDialog: MatDialog,
        private navigationBarService: NavigationBarService,
        private webUserService: WebUserService,
        private webTeamService: WebTeamService,
    ) { }

    ngOnInit() {
        this.refreshData();
    }

    private refreshData(reset: boolean = false) {
        this.loading = true;

        this.webTeamService.getTeams().subscribe((teams: IWebTeamOverview[]) => {
            this.teams = teams;

            this.teamDict = {};
            this.teams.forEach((t) => {
                this.teamDict[t.id as any] = t;
            });

            this.updateLoading();
        });

        this.search();
    }

    private updateLoading() {
        this.loading = !this.teams;
    }

    getTeamId(index: number, item: ITeamOverview) {
        return item.id;
    }

    getUserId(index: number, item: IUserBasicInfo) {
        return item.id;
    }

    setUpMenu(): void {
        this.navigationBarService.setNavMenu(null, 'admin-team-view');
    }

    searchTextChanged(text: string) {
        this.searchText = text;
        clearTimeout(this.searchTimeout);
        this.searchTimeout = setTimeout(() => {
            this.search();
        }, 1000);
    }

    search() {
        this.loadingUsers = true;
        this.webUserService.searchUsers(
            this.searchText && this.searchText !== '' ? this.searchText : undefined,
            this.selectedTeamIds,
            false
        ).subscribe((users: IWebUserOverview[]) => {
            this.loadingUsers = false;
            this.users = users;
        });
    }

    selectTeam(team: IWebTeamOverview) {
        if (team === null) {
            this.selectedTeamIdDict = {};
            this.selectedTeamIds.length = 0;
        } else if (!this.selectedTeamIdDict[team.id as any]) {
            this.selectedTeamIdDict[team.id as any] = true;
            this.selectedTeamIds.push(team.id);
        } else {
            this.selectedTeamIdDict[team.id as any] = false;
            this.selectedTeamIds.splice(this.selectedTeamIds.indexOf(team.id), 1);
        }

        this.search();
    }

    selectUser(user: IUserBasicInfo) {
        this.router.navigate(['./user', user.id], { relativeTo: this.route });
    }

    addNewTeam() {
        const dialogRef = this.matDialog.open(AdminTeamEditComponent, {
            width: '700px',
            data: {
                teamId: null,
                teamName: null
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('closed?');
            this.loading = true;
            this.webTeamService.getTeams().subscribe((teams: IWebTeamOverview[]) => {
                this.loading = false;
                this.teams = teams;
            });
        });
    }

    editTeam(team: IWebTeamOverview) {
        const dialogRef = this.matDialog.open(AdminTeamEditComponent, {
            width: '700px',
            data: {
                teamId: team.id,
                teamName: team.name
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('closed?');
            this.loading = true;
            this.webTeamService.getTeams().subscribe((teams: IWebTeamOverview[]) => {
                this.loading = false;
                this.teams = teams;
            });
        });
    }

    addUser() {
        const dialogRef = this.matDialog.open(AdminTeamUserEditComponent, {
            width: '700px',
            data: {
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            this.search();
        });
    }
}
