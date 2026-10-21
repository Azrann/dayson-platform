import { IWebWorkingDays } from '../shared/web-working-days';
import { IUserId } from './user-id';

export interface IWebUserRules {
    userId: IUserId;

    workingDays: IWebWorkingDays;

    holidayEntitlement: number;
    maxDaysCarriedOverNextYear: number;
    minDaysAdvanceNoticeHolidays: number;
    maxConsecutiveDaysOff: number;
}
