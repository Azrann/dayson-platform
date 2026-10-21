import { ITeamId } from './team-id';
import { ICompanyId } from '../company/company-id';

export interface IWebTeamOverview {
    id: ITeamId;
    companyId: ICompanyId;

    name: string;

    memberCount: number;
}
