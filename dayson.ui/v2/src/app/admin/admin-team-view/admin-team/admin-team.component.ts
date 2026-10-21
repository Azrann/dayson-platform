import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { ITeamOverview } from '../../../shared/services/user/data/ITeamOverview';
import { IUserOverview } from '../../../shared/services/user/data/IUserOverview';
import { UserStatus } from '../../../shared/services/user/data/UserStatus';
import { AdminTeamUserEditComponent } from '../admin-team-user-edit/admin-team-user-edit.component';
import { AdminTeamEditComponent } from '../admin-team-edit/admin-team-edit.component';
import { UserService } from '../../../shared/services/user/user.service';

@Component({
    selector: 'daysonapp-admin-team',
    templateUrl: './admin-team.component.html',
    styleUrls: ['./admin-team.component.scss']
})
export class AdminTeamComponent implements OnInit {
    @Input() team: ITeamOverview;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private matDialog: MatDialog,
        private userService: UserService,
    ) {
    }

    ngOnInit() {
    }

    getMemberId(index: number, item: IUserOverview) {
        return item.id;
    }

    changeUserStatus($event: MouseEvent, user: IUserOverview) {
        if (user.status !== UserStatus.active) {
            user.status = UserStatus.active;
            user.statusText = 'Active';
        } else {
            user.status = UserStatus.inactive;
            user.statusText = 'Inactive';
        }
    }

    openUserDetails(user: IUserOverview) {
        this.router.navigate(['./user', user.id], { relativeTo: this.route });
    }

    addMember() {
        const dialogRef = this.matDialog.open(AdminTeamUserEditComponent, {
            width: '700px',
            data: {
                team: this.team
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });
    }

    editTeam() {
        const dialogRef = this.matDialog.open(AdminTeamEditComponent, {
            width: '700px',
            data: {
                teamId: this.team.id
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('closed?');
            this.userService.getTeamsOverview(true);
        });
    }
}
