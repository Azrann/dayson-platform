import { HolidaysTeamViewComponent } from './holidays-team-view/holidays-team-view.component';
import { ApproveViewComponent } from './approve-view/approve-view.component';
import { Route } from '@angular/router';

import { HolidaysViewComponent } from './holidays-view/holidays-view.component';
import { HolidaysComponent } from './holidays/holidays.component';

export const HolidaysRouting: Route[] = [
    {
        path: 'holidays', component: HolidaysComponent, children: [
            { path: 'view/year/:year', component: HolidaysViewComponent },
            { path: 'approve', component: ApproveViewComponent },
            { path: 'view/user/:userId/year/:year', component: HolidaysViewComponent },
            { path: 'view/team', component: HolidaysTeamViewComponent },
            { path: '', redirectTo: 'holidays/view/year/2017', pathMatch: 'full' }
        ]
    }
];
