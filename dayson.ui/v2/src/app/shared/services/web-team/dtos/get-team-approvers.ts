import { ITeamId } from '../../../data/team/team-id';
import { IWebApproverOverview } from '../../../data/user/web-approver-overview';

export interface IGetTeamApproversRequest {
    teamId: ITeamId;
}

export interface IGetTeamApproversResponse {
    approvers: IWebApproverOverview[];
}
