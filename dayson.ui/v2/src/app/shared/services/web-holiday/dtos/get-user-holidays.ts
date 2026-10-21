import { IUserId } from '../../../data/user/user-id';
import { IWebHolidayOverview } from '../../../data/holiday/web-holiday-overview';

export interface IGetUserHolidaysRequest {
    userId: IUserId;
}

export interface IGetUserHolidaysResponse {
    holidays: IWebHolidayOverview[];
}
