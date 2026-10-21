import { HomeRoutes } from './home.routing';
import { RouterModule } from '@angular/router';
import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeViewComponent } from './home-view/home-view.component';
import { HomeComponent } from './home/home.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild(HomeRoutes),
    ],
    declarations: [
        HomeViewComponent,
        HomeComponent,
    ],
})
export class HomeModule { }
