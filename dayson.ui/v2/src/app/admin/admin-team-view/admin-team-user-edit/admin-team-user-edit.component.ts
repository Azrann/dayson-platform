import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives';
import { MAT_DIALOG_DATA, MatDialogRef, MatSelectChange } from '@angular/material';

import { IWebWorkingDays } from '../../../shared/data/shared/web-working-days';
import { IWebTeamOverview } from '../../../shared/data/team/web-team-overview';
import { IWebApproverOverview } from '../../../shared/data/user/web-approver-overview';
import { IWebUserDetails } from '../../../shared/data/user/web-user-details';
import { IWebUserRules } from '../../../shared/data/user/web-user-rules';
import { WebUserStatus } from '../../../shared/data/user/web-user-status';
import { WebUserType } from '../../../shared/data/user/web-user-type';
import { TeamService } from '../../../shared/services/team/team.service';
import { WebTeamService } from '../../../shared/services/web-team/web-team.service';
import { IUserDetailUpdate } from '../../../shared/services/web-user/dtos/user-detail-update';
import { WebUserService } from '../../../shared/services/web-user/web-user.service';

@Component({
    selector: 'daysonapp-admin-team-user-edit',
    templateUrl: './admin-team-user-edit.component.html',
    styleUrls: ['./admin-team-user-edit.component.scss']
})
export class AdminTeamUserEditComponent implements OnInit {
    user: IWebUserDetails;

    page = -1;

    approvers: IWebApproverOverview[];
    teams: IWebTeamOverview[];

    loading: boolean;
    fileHover: boolean;

    constructor(
        public dialogRef: MatDialogRef<AdminTeamUserEditComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        public teamService: TeamService,
        public webUserService: WebUserService,
        public webTeamService: WebTeamService,
    ) { }

    ngOnInit() {
        this.loading = true;

        this.user = {
            approverId: null,
            approverName: null,
            companyId: null,
            email: null,
            firstName: null,
            id: null,
            isApprover: null,
            jobTitle: null,
            lastName: null,
            middleName: null,
            phone: null,
            pictureUrl: null,
            rules: {
                holidayEntitlement: 25,
                maxConsecutiveDaysOff: 20,
                maxDaysCarriedOverNextYear: 5,
                minDaysAdvanceNoticeHolidays: 15,
                userId: null,
                workingDays: {
                    monday: true,
                    tuesday: true,
                    wednesday: true,
                    thursday: true,
                    friday: true,
                    saturday: false,
                    sunday: false,
                } as IWebWorkingDays
            } as IWebUserRules,
            status: WebUserStatus.Inactive,
            teamId: null,
            teamName: null,
            type: WebUserType.User
        } as IWebUserDetails;

        this.user.type = WebUserType.User;
        this.user.isApprover = false;

        this.webTeamService.getTeams().subscribe((teams) => {
            this.loading = false;
            this.teams = teams;
            this.page = 0;
            this.user.teamId = teams[0].id;

            this.refreshApprovers();
        });
    }

    refreshApprovers() {
        this.loading = true;
        this.webTeamService.getTeamApprovers(this.user.teamId).subscribe(approvers => {
            this.approvers = approvers;
            console.log('received approvers:', approvers);
            if (approvers.length) {
                this.user.approverId = this.approvers[0].id;
            } else {
                this.user.approverId = '' as any;
            }

            this.loading = false;
        });
    }

    close() {
        this.dialogRef.close();
    }

    next() {
        this.page++;
    }

    prev() {
        this.page--;
    }

    create(userDetailsForm: NgForm) {
        const userDetailUpdate = {
            approverId: this.user.approverId,
            email: this.user.email,
            firstName: this.user.firstName,
            isApprover: this.user.isApprover,
            jobTitle: this.user.jobTitle,
            lastName: this.user.lastName,
            middleName: this.user.middleName,
            phone: this.user.phone,
            pictureUrl: this.user.pictureUrl,
            rules: {
                holidayEntitlement: this.user.rules.holidayEntitlement,
                maxConsecutiveDaysOff: this.user.rules.maxConsecutiveDaysOff,
                maxDaysCarriedOverNextYear: this.user.rules.maxDaysCarriedOverNextYear,
                minDaysAdvanceNoticeHolidays: this.user.rules.minDaysAdvanceNoticeHolidays,
                workingDays: {
                    monday: this.user.rules.workingDays.monday,
                    tuesday: this.user.rules.workingDays.tuesday,
                    wednesday: this.user.rules.workingDays.wednesday,
                    thursday: this.user.rules.workingDays.thursday,
                    friday: this.user.rules.workingDays.friday,
                    saturday: this.user.rules.workingDays.saturday,
                    sunday: this.user.rules.workingDays.sunday
                } as IWebWorkingDays
            } as IWebUserRules,
            teamId: this.user.teamId,
            type: this.user.type,
            userId: null
        } as IUserDetailUpdate;

        this.loading = true;

        this.webUserService.createUser(userDetailUpdate).subscribe((result) => {
            this.close();
        });
    }

    canSave(form: NgForm): boolean {
        return form.valid;
    }

    fileChanged(event: Event) {
        const target: HTMLInputElement = <HTMLInputElement>event.target;
        const file = target.files.length ? target.files[0] : null;

        if (!file) { return; }

        this.changeImage(file);
    }

    changeImage(file: File) {
        const reader = new FileReader();

        reader.onload = (e: any) => {
            const dataUrl = e.target.result;
            // this.user.imageUrl = dataUrl;
        };

        reader.readAsDataURL(file);
    }

    onDrop(event: DragEvent) {
        // console.log('onDrop:', event);
        this.fileHover = false;
        if (!/^image\//.test(event.dataTransfer.items[0].type)) {
            return;
        }

        event.preventDefault();

        if (event.dataTransfer.files && event.dataTransfer.files.length) {
            this.changeImage(event.dataTransfer.files[0]);
        }
    }

    onDragEnter(event: DragEvent) {
        // console.log('onDragEnter:', event);

        if (/^image\//.test(event.dataTransfer.items[0].type)) {
            this.fileHover = true;
        }
    }

    onDragLeave(event: DragEvent) {
        // console.log('onDragLeave:', event);
        this.fileHover = false;
    }

    onDragOver(event: DragEvent) {
        // console.log('onDragOver:', event);

        if (/^image\//.test(event.dataTransfer.items[0].type)) {
            event.preventDefault();
        }
    }

    teamChanged(event: MatSelectChange) {
        this.refreshApprovers();
    }
}
