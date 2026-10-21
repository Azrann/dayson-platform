import { Route } from '@angular/router';

import { AdminCompanyViewComponent } from './admin-company-view/admin-company-view.component';
import { AdminTeamDetailsViewComponent } from './admin-team-details-view/admin-team-details-view.component';
import { AdminUserDetailsViewComponent } from './admin-user-details-view/admin-user-details-view.component';
import { AdminTeamViewComponent } from './admin-team-view/admin-team-view.component';
import { AdminComponent } from './admin/admin.component';

export const AdminRoutes: Route[] = [
    {
        path: 'admin', redirectTo: 'admin/company'
    },
    {
        path: 'admin', component: AdminComponent, children: [
            { path: 'company', component: AdminCompanyViewComponent },
            { path: 'teams', component: AdminTeamViewComponent },
            { path: 'profile', component: AdminUserDetailsViewComponent },
            { path: 'teams/user/:userId', component: AdminUserDetailsViewComponent },
            { path: 'teams/team/:teamId', component: AdminTeamDetailsViewComponent },
            { path: '', redirectTo: 'admin/teams', pathMatch: 'full' },
            // { path: '**', redirectTo: 'admin/company', pathMatch: 'full' }
        ]
    },
];