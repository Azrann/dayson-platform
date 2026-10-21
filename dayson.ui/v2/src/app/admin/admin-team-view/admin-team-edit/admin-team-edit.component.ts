import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { IWebTeamOverview } from '../../../shared/data/team/web-team-overview';
import { WebTeamService } from '../../../shared/services/web-team/web-team.service';

@Component({
    selector: 'daysonapp-admin-team-edit',
    templateUrl: './admin-team-edit.component.html',
    styleUrls: ['./admin-team-edit.component.scss']
})
export class AdminTeamEditComponent implements OnInit {
    team: IWebTeamOverview;
    loading: boolean;

    constructor(
        public dialogRef: MatDialogRef<any>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public webTeamService: WebTeamService,
    ) { }

    ngOnInit() {
        if (!this.data.teamId) {
            this.team = {
                id: null,
                name: '',
            } as IWebTeamOverview;
        } else {
            this.team = {
                id: this.data.teamId,
                name: this.data.teamName
            } as IWebTeamOverview;

            this.loading = true;

            this.webTeamService.getTeams().subscribe((teams: IWebTeamOverview[]) => {
                this.loading = false;
                this.team = teams.find((t) => t.id === this.data.teamId);
            });
        }
    }

    save(form: NgForm) {
        this.loading = true;
        if (!this.team.id) {
            this.webTeamService.createTeam(form['teamName']).subscribe(() => {
                this.loading = false;
                this.dialogRef.close(true);
            });
        } else {
            this.webTeamService.updateTeam(this.team.id, form['teamName']).subscribe(() => {
                this.loading = false;
                this.dialogRef.close(true);
            });
        }
    }
}
