import { IWebTeamOverview } from '../../../data/team/web-team-overview';

export interface IGetTeamsRequest {
    _IGetTeamsRequest?: any;
}

export interface IGetTeamsResponse {
    teams: IWebTeamOverview[];
}
