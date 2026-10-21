import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { ApproveViewComponent } from './approve-view/approve-view.component';
import { CalendarDayComponent } from './calendar/calendar-day.component';
import { CalendarMonthComponent } from './calendar/calendar-month.component';
import { CalendarYearComponent } from './calendar/calendar-year.component';
import { HolidaysTeamViewComponent } from './holidays-team-view/holidays-team-view.component';
import { HolidaysUserSelectorComponent } from './holidays-team-view/holidays-user-selector/holidays-user-selector.component';
import { HolidaySideBarComponent } from './holidays-view/holiday-side-bar/holiday-side-bar.component';
import { HolidaysViewComponent } from './holidays-view/holidays-view.component';
import {
    RequestedHolidayButtonComponent,
} from './holidays-view/requested-holiday-button/requested-holiday-button.component';
import {
    RequestedHolidayDetailsComponent,
} from './holidays-view/requested-holiday-details/requested-holiday-details.component';
import { HolidaysMonthComponent } from './holidays-year/holidays-month/holidays-month.component';
import { HolidaysYearComponent } from './holidays-year/holidays-year.component';
import { HolidaysRouting } from './holidays.routing';
import { HolidaysComponent } from './holidays/holidays.component';

@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild(HolidaysRouting),
    ],
    providers: [
    ],
    declarations: [
        HolidaysComponent,
        HolidaysViewComponent,
        HolidaysMonthComponent,
        HolidaysYearComponent,
        HolidaySideBarComponent,
        RequestedHolidayButtonComponent,
        RequestedHolidayDetailsComponent,
        ApproveViewComponent,
        HolidaysTeamViewComponent,
        HolidaysUserSelectorComponent,

        // calendar
        CalendarYearComponent,
        CalendarMonthComponent,
        CalendarDayComponent,
    ],
    entryComponents: [
        RequestedHolidayDetailsComponent,
    ]
})
export class HolidaysModule { }
