import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { IWebTeamOverview } from '../../shared/data/team/web-team-overview';
import { IUserId } from '../../shared/data/user/user-id';
import { IWebApproverOverview } from '../../shared/data/user/web-approver-overview';
import { IWebUserDetails } from '../../shared/data/user/web-user-details';
import { IWebUserOverview } from '../../shared/data/user/web-user-overview';
import { NavigationBarService } from '../../shared/navigation-bar/navigration-bar.service';
import { CompanyService } from '../../shared/services/company/company.service';
import { IUserOverview } from '../../shared/services/user/data/IUserOverview';
import { WebTeamService } from '../../shared/services/web-team/web-team.service';
import { IUserDetailUpdate } from '../../shared/services/web-user/dtos/user-detail-update';
import { WebUserService } from '../../shared/services/web-user/web-user.service';
import { UserService } from './../../shared/services/user/user.service';

@Component({
    selector: 'daysonapp-admin-user-details-view',
    templateUrl: './admin-user-details-view.component.html',
    styleUrls: ['./admin-user-details-view.component.scss']
})
export class AdminUserDetailsViewComponent implements OnInit {
    user: IWebUserDetails;
    teams: IWebTeamOverview[];
    approvers: IWebApproverOverview[];

    initials: string;

    loading = true;

    mode = 'information';

    fileHover = false;

    // navigation
    previousUser: IWebUserOverview;
    nextUser: IWebUserOverview;

    constructor(
        private activatedRoute: ActivatedRoute,
        private userService: UserService,
        private navigationBarService: NavigationBarService,
        private router: Router,
        private webUserService: WebUserService,
        private webTeamService: WebTeamService,
        private companyService: CompanyService,
    ) { }

    ngOnInit() {

        this.activatedRoute.params.subscribe((params) => {
            const userId: IUserId = params['userId'];

            if (!userId) {
                this.webUserService.getCurrentUserDetails().subscribe((user: IWebUserDetails) => {
                    this.user = user;
                    this.loadData();
                });
            } else {
                this.webUserService.getUserDetails(userId).subscribe((user: IWebUserDetails) => {
                    this.user = user;
                    this.loadData();
                });
            }

            this.navigationBarService.setNavMenu(null, userId ? 'admin-user-details-view-user' : 'admin-user-details-view-profile');
        });
    }

    private loadData() {
        this.loading = true;

        this.setupNavigation();

        this.webTeamService.getTeams().subscribe((teams: IWebTeamOverview[]) => {
            this.teams = teams;
            this.updateLoading();
        });

        this.webTeamService.getTeamApprovers(this.user.teamId).subscribe((approvers: IWebApproverOverview[]) => {
            this.approvers = approvers;
            this.updateLoading();
        });
    }

    private setupNavigation() {
        this.previousUser = null;
        this.nextUser = null;

        console.warn('navigation not done yet');

        // this.userService.getTeam(this.user.myTeam.id).subscribe((team) => {
        //     const i = team.teamMembers.findIndex(u => u.id === this.user.id);

        //     if (i !== 0) {
        //         this.previousUser = team.teamMembers[i - 1];
        //     }

        //     if (i !== team.teamMembers.length - 1) {
        //         this.nextUser = team.teamMembers[i + 1];
        //     }
        // });
    }

    goToUser(user: IUserOverview) {
        this.router.navigate(['admin', 'teams', 'user', user.id]);
        // this.loadData(user.id);
    }

    updateLoading() {
        this.loading = !this.teams || !this.approvers || !this.user;
        this.updateInitials();
    }

    saveUserData(formValues: any) {
        console.log('saveUserData', formValues);
        this.webUserService.updateUser(this.user.id, this.generateUserUpdate()).subscribe((response) => {
            console.log('user basic details updated, response:', response);
        });
    }

    private generateUserUpdate(): IUserDetailUpdate {
        return {
            approverId: this.user.approverId,
            email: this.user.email,
            firstName: this.user.firstName,
            isApprover: this.user.isApprover,
            jobTitle: this.user.jobTitle,
            lastName: this.user.lastName,
            middleName: this.user.middleName,
            pictureUrl: this.user.pictureUrl,
            phone: this.user.phone,
            rules: this.user.rules,
            teamId: this.user.teamId,
            type: this.user.type,
            userId: this.user.id
        } as IUserDetailUpdate;
    }

    private updateInitials() {
        this.initials =
            ((this.user.firstName && this.user.firstName.length ? this.user.firstName.charAt(0) : '') +
                (this.user.lastName && this.user.lastName.length ? this.user.lastName.charAt(0) : '')).toLocaleUpperCase();
    }

    fileChanged(event: Event) {
        const target: HTMLInputElement = <HTMLInputElement>event.target;
        const file = target.files.length ? target.files[0] : null;

        if (!file) { return; }

        this.changeImage(file);
    }

    changeImage(file: File) {
        throw new Error('Not implemented');

        // const reader = new FileReader();

        // reader.onload = (e: any) => {
        //     const dataUrl = e.target.result;
        //     this.user.imageUrl = dataUrl;
        // };

        // reader.readAsDataURL(file);
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
}
