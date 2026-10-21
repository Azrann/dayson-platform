import { CommonModule } from '@angular/common';
import { LOCALE_ID, NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormControl, FormGroupDirective, FormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms/src/directives';
import { Http, HttpModule, RequestOptions } from '@angular/http';
import {
    MatButtonModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatIconModule,
    MatInputModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatSelectModule,
    MatSidenavModule,
    MatSlideToggleModule,
} from '@angular/material';
import { MAT_MOMENT_DATE_FORMATS, MatMomentDateModule, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, ErrorStateMatcher, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthConfig, AuthHttp } from 'angular2-jwt';
import { MomentModule } from 'angular2-moment';
import { PubNubAngular } from 'pubnub-angular2';

import { HeaderComponent } from './header/header.component';
import { ImageFileSelectorComponent } from './image-file-selector/image-file-selector.component';
import { LoadingSplashComponent } from './loading-splash/loading-splash.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { NavigationBarService } from './navigation-bar/navigration-bar.service';
import { NavigationMenuComponent } from './navigation-menu/navigation-menu.component';
import { AdminService } from './services/admin.service';
import { AuthUserService } from './services/auth-user/auth-user.service';
import { CompanyService } from './services/company/company.service';
import { HolidayManagerService } from './services/holidays/holiday-manager.service';
import { HolidaysService } from './services/holidays/holidays.service';
import { WebDateService } from './services/infrastructure/web-date.service';
import { TeamService } from './services/team/team.service';
import { UserService } from './services/user/user.service';
import { WebCompanyService } from './services/web-company/web-company.service';
import { WebHolidayService } from './services/web-holiday/web-holiday.service';
import { WebTeamService } from './services/web-team/web-team.service';
import { WebUserService } from './services/web-user/web-user.service';
import { RepeatDirective } from './validators/repeat.directive';
import { ValidationRefreshDirective } from './validators/validation-refresh.directive';

export function authHttpServiceFactory(http: Http, options: RequestOptions) {
    return new AuthHttp(new AuthConfig({
        tokenName: 'token',
        tokenGetter: (() => sessionStorage.getItem('dayson-token')),
        globalHeaders: [{ 'Content-Type': 'application/json' }],
    }), http, options);
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
    isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
        const isSubmitted = form && form.submitted;
        return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
    }
}

@NgModule({
    imports: [
        BrowserModule,
        CommonModule,
        FormsModule,
        HttpModule,

        FlexLayoutModule,

        MomentModule,

        // material stuff
        MatNativeDateModule,
        MatButtonModule, MatCheckboxModule, MatInputModule, MatDialogModule, MatSelectModule, MatExpansionModule,
        MatSidenavModule, MatDatepickerModule, MatSlideToggleModule, MatIconModule, MatProgressBarModule,
        MatMomentDateModule,
    ],
    providers: [
        CompanyService,
        AuthUserService,
        HolidaysService,
        HolidayManagerService,
        AdminService,
        {
            provide: AuthHttp,
            useFactory: authHttpServiceFactory,
            deps: [Http, RequestOptions]
        },
        NavigationBarService,
        UserService,
        TeamService,

        WebDateService,

        WebCompanyService,
        WebUserService,
        WebTeamService,
        WebHolidayService,

        { provide: ErrorStateMatcher, useClass: MyErrorStateMatcher },
        { provide: LOCALE_ID, useValue: 'es' },
        { provide: MAT_DATE_LOCALE, useValue: 'es' },
        { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },

        PubNubAngular,
    ],
    declarations: [
        HeaderComponent,
        LoadingSplashComponent,
        NavigationBarComponent,
        NavigationMenuComponent,
        ImageFileSelectorComponent,
        RepeatDirective,
        ValidationRefreshDirective,
    ],
    exports: [
        FormsModule,
        CommonModule,
        BrowserModule,
        HttpModule,

        HeaderComponent,
        LoadingSplashComponent,
        NavigationBarComponent,
        NavigationMenuComponent,
        ImageFileSelectorComponent,

        RepeatDirective,
        ValidationRefreshDirective,

        // @angular/flex-layout
        FlexLayoutModule,


        // @angular/material
        MatNativeDateModule,
        MatButtonModule, MatCheckboxModule, MatInputModule, MatDialogModule, MatSelectModule, MatExpansionModule,
        MatSidenavModule, MatDatepickerModule, MatSlideToggleModule, MatIconModule, MatProgressBarModule,
    ]
})
export class SharedModule { }
