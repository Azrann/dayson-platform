import { IHolidayId } from '../../../data/holiday/holiday-id';
import { IWebHolidayOverview } from '../../../data/holiday/web-holiday-overview';
import { WebHolidayStatus } from '../../../data/holiday/web-holiday-status';

export interface IChangeHolidayStatusRequest {
    holidayId: IHolidayId;

    status: WebHolidayStatus;
}

export interface IChangeHolidayStatusResponse {
    holiday: IWebHolidayOverview;
}
