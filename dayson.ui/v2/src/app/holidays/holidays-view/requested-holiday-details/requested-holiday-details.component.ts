import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgForm } from '@angular/forms/src/directives';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { IWebHolidayOverview } from '../../../shared/data/holiday/web-holiday-overview';
import { IWebUserOverview } from '../../../shared/data/user/web-user-overview';
import { WebDateService } from '../../../shared/services/infrastructure/web-date.service';
import { WebHolidayService } from '../../../shared/services/web-holiday/web-holiday.service';
import * as moment from 'moment';

@Component({
    selector: 'daysonapp-requested-holiday-details',
    templateUrl: './requested-holiday-details.component.html',
    styleUrls: ['./requested-holiday-details.component.scss']
})
export class RequestedHolidayDetailsComponent implements OnInit {
    @Input() holiday: IWebHolidayOverview;
    @Input() user: IWebUserOverview;
    @Input() year: number;

    holidayDetailsForm: FormGroup;

    minStartDate: moment.Moment;
    maxStartDate: moment.Moment;
    minEndDate: moment.Moment;
    maxEndDate: moment.Moment;

    years: number[];

    error: string;

    constructor(
        public dialogRef: MatDialogRef<RequestedHolidayDetailsComponent>,
        public webHolidayService: WebHolidayService,
        public webDateService: WebDateService,
        @Inject(MAT_DIALOG_DATA) public data: any,
    ) { }

    ngOnInit() {
        this.holiday = this.data.requestedHoliday;
        this.user = this.data.user;
        this.year = this.data.year;

        if (!this.user) {
            throw new Error('user must be set in order to use the requested-holiday-details component');
        }

        if (!this.holiday) {
            throw new Error('requestedHoliday must be set in order to use the requested-holiday-details component');
        }

        this.years = [this.year];
        // for (let i = 0; i < 5; i++) {
        //     this.years.push(this.year - 2 + i);
        // }

        this.setDateBoundaries();
    }

    private setDateBoundaries(form?: NgForm) {
        this.minEndDate = this.minStartDate = moment({ year: this.year, month: 1, date: 1});
        this.maxEndDate = this.maxStartDate = moment({ year: this.year, month: 12, date: 31});

        if (form) {
            if (this.holiday.dateFrom) {
                this.minEndDate = this.holiday.dateFrom;
            }

            if (this.holiday.dateTo) {
                this.maxStartDate = this.holiday.dateTo;
            }
        }
    }

    close(created?: boolean) {
        this.dialogRef.close(created);
    }

    canSave(form: NgForm) {
        this.setDateBoundaries(form);

        // console.log(form);
        return form.valid;
    }

    create(form: NgForm) {
        console.log(form);
        console.log(this.holiday);

        if (!form.valid) {
            return;
        }

        const from = this.webDateService.toWebDate(this.holiday.dateFrom);
        const to = this.webDateService.toWebDate(this.holiday.dateTo);

        this.webHolidayService.createHoliday(
            this.user.id,
            this.holiday.notes,
            from,
            to,
        ).subscribe(() => {
            this.close(true);
        });
    }
}
