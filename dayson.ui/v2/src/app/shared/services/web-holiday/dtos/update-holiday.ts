import { IHolidayId } from '../../../data/holiday/holiday-id';
import { IWebHolidayOverview } from '../../../data/holiday/web-holiday-overview';
import { IWebDate } from '../../../data/shared/web-date';

export interface IUpdateHolidayRequest {
    holidayId: IHolidayId;

    notes: string;
    from: IWebDate;
    to: IWebDate;
}

export interface IUpdateHolidayResponse {
    holiday: IWebHolidayOverview;
}
