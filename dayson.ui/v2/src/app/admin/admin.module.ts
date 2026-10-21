import { AdminTeamEditComponent } from './admin-team-view/admin-team-edit/admin-team-edit.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { AdminCompanyViewComponent } from './admin-company-view/admin-company-view.component';
import { AdminTeamDetailsViewComponent } from './admin-team-details-view/admin-team-details-view.component';
import { AdminUserDetailsViewComponent } from './admin-user-details-view/admin-user-details-view.component';
import { AdminTeamUserEditComponent } from './admin-team-view/admin-team-user-edit/admin-team-user-edit.component';
import { AdminTeamViewComponent } from './admin-team-view/admin-team-view.component';
import { AdminTeamComponent } from './admin-team-view/admin-team/admin-team.component';
import { AdminRoutes } from './admin.routing';
import { AdminComponent } from './admin/admin.component';

@NgModule({
    imports: [
        RouterModule.forChild(AdminRoutes),
        SharedModule
    ],
    declarations: [
        AdminComponent,
        AdminCompanyViewComponent,
        AdminTeamComponent,
        AdminTeamViewComponent,
        AdminTeamEditComponent,
        AdminTeamComponent,
        AdminTeamViewComponent,
        AdminTeamUserEditComponent,
        AdminUserDetailsViewComponent,
        AdminTeamDetailsViewComponent,
    ],
    entryComponents: [
        AdminTeamUserEditComponent,
        AdminTeamEditComponent,
    ]
})
export class AdminModule { }
