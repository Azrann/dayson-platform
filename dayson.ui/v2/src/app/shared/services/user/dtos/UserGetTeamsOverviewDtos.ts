import { ITeamOverview } from './../data/ITeamOverview';

export interface IUserGetTeamsOverviewResponseDto {
    teams: ITeamOverview[];
    canAddNewTeams: boolean;
}
