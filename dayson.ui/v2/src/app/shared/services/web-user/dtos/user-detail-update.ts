import { ITeamId } from '../../../data/team/team-id';
import { IUserId } from '../../../data/user/user-id';
import { IWebUserRules } from '../../../data/user/web-user-rules';
import { WebUserType } from '../../../data/user/web-user-type';

export interface IUserDetailUpdate {
    // different from backend dtos
    userId?: IUserId;

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
    isApprover: boolean;

    rules: IWebUserRules;
}
