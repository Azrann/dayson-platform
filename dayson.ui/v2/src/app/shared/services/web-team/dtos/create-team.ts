import { IWebTeamOverview } from '../../../data/team/web-team-overview';

export interface ICreateTeamRequest {
    name: string;
}

export interface ICreateTeamResponse {
    team: IWebTeamOverview;
}
