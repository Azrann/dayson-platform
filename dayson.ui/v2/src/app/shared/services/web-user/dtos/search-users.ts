import { IWebUserOverview } from '../../../data/user/web-user-overview';
import { ITeamId } from '../../../data/team/team-id';

export interface ISearchUsersRequest {
    text: string;
    teamIds: ITeamId[];
    onlyActive: boolean;
}

export interface ISearchUsersResponse {
    users: IWebUserOverview[];
}
