import { WebUserStatus } from './web-user-status';
import { WebUserType } from './web-user-type';
import { ITeamId } from '../team/team-id';
import { IUserId } from './user-id';
import { ICompanyId } from '../company/company-id';

export interface IWebUserOverview {
    id: IUserId;
    companyId: ICompanyId;
    teamId: ITeamId;
    approverId: IUserId;

    firstName: string;
    middleName: string;
    lastName: string;
    jobTitle: string;
    email: string;
    phone: string;
    pictureUrl: string;

    type: WebUserType;
    status: WebUserStatus;

    isApprover: boolean;
}
