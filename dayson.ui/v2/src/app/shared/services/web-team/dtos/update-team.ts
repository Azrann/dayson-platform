import { ITeamId } from '../../../data/team/team-id';
import { IWebTeamOverview } from '../../../data/team/web-team-overview';

export interface IUpdateTeamRequest {
    teamId: ITeamId;
    name: string;
}

export interface IUpdateTeamResponse {
    team: IWebTeamOverview;
}
