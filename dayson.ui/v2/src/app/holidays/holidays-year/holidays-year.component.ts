import { Component, Input } from '@angular/core';

import { HolidaysService } from '../../shared/services/holidays/holidays.service';
import { IUserOverview } from '../../shared/services/user/data/IUserOverview';
import { HolidayYear } from './data/holiday-year';

@Component({
    selector: 'daysonapp-holidays-year',
    templateUrl: './holidays-year.component.html',
    styleUrls: ['./holidays-year.component.scss']
})
export class HolidaysYearComponent {
    @Input() holidayYear: HolidayYear;

    constructor(
        private holidayService: HolidaysService
    ) { }
}
