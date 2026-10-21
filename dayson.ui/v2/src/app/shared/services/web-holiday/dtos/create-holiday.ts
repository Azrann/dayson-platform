import { IWebHolidayOverview } from '../../../data/holiday/web-holiday-overview';
import { IWebDate } from '../../../data/shared/web-date';
import { IUserId } from '../../../data/user/user-id';

export interface ICreateHolidayRequest {
    userId: IUserId;

    notes: string;
    from: IWebDate;
    to: IWebDate;
}

export interface ICreateHolidayResponse {
    _ICreateHolidayResponseBrand?: string;
}
