import { ITeamMember } from '../../user/data/ITeamMember';
import { IManager } from '../../user/data/IManager';

export interface ITeamBasicInfo {
    id: string;
    name: string;

    manager: IManager;
    temporalManager: IManager;
    memebers: ITeamMember[];
}
