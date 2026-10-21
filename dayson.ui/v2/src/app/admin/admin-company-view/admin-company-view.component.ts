import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { IWebCompanyRules } from '../../shared/data/company/web-company-rules';
import { IWebDate } from '../../shared/data/shared/web-date';
import { NavigationBarService } from '../../shared/navigation-bar/navigration-bar.service';
import { CompanyCountry } from '../../shared/services/company/data/company-country';
import { WebCompanyService } from '../../shared/services/web-company/web-company.service';
import { Month } from './month';
import { WeekDay } from './week-day';

@Component({
    selector: 'daysonapp-admin-view-company',
    templateUrl: './admin-company-view.component.html',
    styleUrls: ['./admin-company-view.component.scss']
})
export class AdminCompanyViewComponent implements OnInit, OnDestroy {
    days: IDaySelection[];
    // companySizes: CompanySize[];
    companyCountries: CompanyCountry[];
    startMonths: Month[];
    dayAmounts: number[];

    company: IWebCompanyRules;

    daysChanged = false;
    originalStartMonth: number;
    startMonth: number;

    loading: boolean;
    destroyed: boolean;

    constructor(
        private router: Router,
        private webCompanyService: WebCompanyService,
        private navigationBarService: NavigationBarService,
    ) { }

    ngOnInit() {
        this.loading = true;

        console.log('getting rules in company');
        this.webCompanyService.getCompanyRules().subscribe((company: IWebCompanyRules) => {
            this.loading = false;

            this.setPageData(company);
        });
    }

    ngOnDestroy() {
        this.destroyed = true;
    }

    private saveData(info: any) {
        // this.company.name = info.name;
        // this.company.countryId = info.countryId;
        // this.company.openDuringBankHolidays = info.openDuringBankHolidays;
        this.company.workingDays.monday = this.days[0].open;
        this.company.workingDays.tuesday = this.days[1].open;
        this.company.workingDays.wednesday = this.days[2].open;
        this.company.workingDays.thursday = this.days[3].open;
        this.company.workingDays.friday = this.days[4].open;
        this.company.workingDays.saturday = this.days[5].open;
        this.company.workingDays.sunday = this.days[6].open;

        this.company.maxDaysCarriedOverNextYear = info.maxDaysCarriedOverNextYear;
        this.company.maxConsecutiveDaysOff = info.maxConsecutiveDaysOff;
        this.company.holidayEntitlement = info.defaultNumberOfHolidays;
        this.company.minDaysAdvanceNoticeHolidays = info.minDaysAdvanceNoticeHolidays;
        this.company.yearStartDay = {
            year: 2018,
            month: this.startMonth,
            day: 1
        } as IWebDate;

        this.webCompanyService.updateCompanyRules(this.company).subscribe((rules: IWebCompanyRules) => {
            this.daysChanged = false;
            this.originalStartMonth = info.startMonth;

            this.setPageData(rules);

        }, (error) => {
            console.error(error);
        });
    }

    private setPageData(rules: IWebCompanyRules) {
        if (this.destroyed) {
            return;
        }

        this.company = rules;

        // if (this.company.sizeId === undefined) {
        //     this.company.sizeId = 1;
        // }

        // if (this.company.countryId === undefined) {
        //     this.company.countryId = 'gb';
        // }

        this.days = [];
        WeekDay.days.forEach(day => {
            this.days.push(<IDaySelection>{
                day: day,
            });
        });

        this.days[0].open = this.company.workingDays.monday;
        this.days[1].open = this.company.workingDays.tuesday;
        this.days[2].open = this.company.workingDays.wednesday;
        this.days[3].open = this.company.workingDays.thursday;
        this.days[4].open = this.company.workingDays.friday;
        this.days[5].open = this.company.workingDays.saturday;
        this.days[6].open = this.company.workingDays.sunday;

        try {
            const d = new Date(new Date().getFullYear(), this.company.yearStartDay.month, 1);
            this.startMonth = d.getMonth() + 1;
        } catch {
            this.startMonth = 0;
        }

        this.originalStartMonth = this.startMonth;

        // this.companySizes = CompanySize.sizes;
        this.companyCountries = CompanyCountry.countries;
        this.startMonths = Month.months;

        this.dayAmounts = [];
        for (let i = 0; i < 60; i++) {
            this.dayAmounts.push(i);
        }

        if (this.company.maxConsecutiveDaysOff > this.dayAmounts.length) {
            this.company.maxConsecutiveDaysOff = this.dayAmounts.length - 1;
        }

        // if (this.company.initialized) {
             this.navigationBarService.setNavMenu(null, null);
        // } else {
            // this.navigationBarService.setNavMenu(null, 'admin-company-view');
        // }
    }

    confirmRegistration(info: any) {
        this.saveData(info);

        this.router.navigate(['admin', 'teams']);
    }
}

interface IDaySelection {
    day: WeekDay;
    open: boolean;
}
