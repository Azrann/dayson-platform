import { IWebTeamOverview } from '../team/web-team-overview';
import { IWebApproverOverview } from './web-approver-overview';
import { IWebUserRules } from './web-user-rules';
import { IWebUserOverview } from './web-user-overview';

export interface IWebUserDetails extends IWebUserOverview {
    rules: IWebUserRules;

    approverName: string;

    teamName: string;
}
