import { HomeComponent } from './home/home.component';
import { HomeViewComponent } from './home-view/home-view.component';
import { Route } from '@angular/router';

export const HomeRoutes: Route[] = [
    {
        path: 'home', redirectTo: 'home/'
    },
    {
        path: 'home', component: HomeComponent, children: [
            { path: '', component: HomeViewComponent },
            // { path: 'teams/team/:teamId', component: AdminTeamTeamDetailsViewComponent },
            // { path: '**', redirectTo: 'admin/company', pathMatch: 'full' }
        ]
    },
];
