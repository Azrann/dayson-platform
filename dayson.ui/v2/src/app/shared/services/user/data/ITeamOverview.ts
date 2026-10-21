import { IUserBasicInfo } from '../../auth-user/data/AuthUser';

export interface ITeamOverview {
    id: string;
    name: string;

    manager: IUserBasicInfo;
    temporalManager: IUserBasicInfo;
    teamMembers: IUserBasicInfo[];
}
