import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';

import { AdminModule } from './admin/admin.module';
import { DaysonComponent } from './dayson.component';
import { DaysonRoutes } from './dayson.routing';
import { HolidaysModule } from './holidays/holidays.module';
import { HomeModule } from './home/home.module';
import { LoginModule } from './login/login.module';
import { SharedModule } from './shared/shared.module';

import localeEnGb from '@angular/common/locales/en-GB';
import localeEs from '@angular/common/locales/es';
import { registerLocaleData } from '@angular/common';
import { MAT_DATE_LOCALE } from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

// Depending on whether rollup is used, moment needs to be imported differently.
// Since Moment.js doesn't have a default export, we normally need to import using the `* as`
// syntax. However, rollup creates a synthetic default module and we thus need to import it using
// the `default as` syntax.
import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports

const moment = _moment;

// registerLocaleData(localeEnGb, 'en-GB');
registerLocaleData(localeEs, 'es');

@NgModule({
  imports: [
    RouterModule.forRoot(DaysonRoutes),
    BrowserAnimationsModule,
    MatMomentDateModule,
    LoginModule,
    AdminModule,
    SharedModule,
    HolidaysModule,
    HomeModule,
  ],
  exports: [
  ],
  declarations: [
    DaysonComponent,
  ],
  providers: [
  ],
  bootstrap: [DaysonComponent],
})
export class DaysonModule { }
