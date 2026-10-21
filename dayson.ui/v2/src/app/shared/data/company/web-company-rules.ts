import { IWebDate } from '../shared/web-date';
import { IWebWorkingDays } from '../shared/web-working-days';

export interface IWebCompanyRules {
    id: string;
    companyId: string;

    yearStartDay: IWebDate;

    workingDays: IWebWorkingDays;

    holidayEntitlement: number;
    maxDaysCarriedOverNextYear: number;
    minDaysAdvanceNoticeHolidays: number;
    maxConsecutiveDaysOff: number;
}
