import { ICompanyId } from '../company/company-id';
import { IWebDate } from '../shared/web-date';
import { IUserId } from '../user/user-id';
import { IHolidayId } from './holiday-id';
import { WebHolidayStatus } from './web-holiday-status';
import { Moment } from 'moment';

export interface IWebHolidayOverview {
    id: IHolidayId;
    companyId: ICompanyId;
    userId: IUserId;

    notes: string;
    from: IWebDate;
    to: IWebDate;
    numberOfDays?: number;

    status: WebHolidayStatus;

    // ui specific fields
    dateFrom?: Moment;
    dateTo?: Moment;
    index?: number;
}
