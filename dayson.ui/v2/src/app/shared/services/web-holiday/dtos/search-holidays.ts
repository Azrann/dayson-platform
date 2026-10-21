import { IHolidayId } from '../../../data/holiday/holiday-id';
import { IWebHolidayOverview } from '../../../data/holiday/web-holiday-overview';
import { WebHolidayStatus } from '../../../data/holiday/web-holiday-status';
import { ITeamId } from '../../../data/team/team-id';
import { IWebDate } from '../../../data/shared/web-date';
import { IUserId } from '../../../data/user/user-id';

export interface ISearchHolidaysRequest {
    teamIds: ITeamId[];
    userIds: IUserId[];
    from: IWebDate;
    to: IWebDate;
    status: WebHolidayStatus;
}

export interface ISearchHolidaysResponse {
    holidays: IWebHolidayOverview[];
}
