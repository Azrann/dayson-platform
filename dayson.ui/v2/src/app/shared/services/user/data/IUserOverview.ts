import { RequestedHoliday } from '../../holidays/data/requested-holiday';
import { UserType } from './UserType';
import { UserStatus } from './UserStatus';
import { UserWorkingDays } from './UserWorkingDays';

export interface IUserOverview {
    id: string;

    imageUrl: string;
    firstname: string;
    middlename: string;
    lastname: string;
    jobTitle: string;
    email: string;
    hasApprovalRights: boolean;
    account: UserType;
    status: UserStatus;
    approverId: string;
    teamId: string;
    workingDays: UserWorkingDays;
    holidayAllowance: number;
    carryAllowance: number;
    remainingAllowance: number;
    phone: string;
    companyId: string;
    companyName: string;

    // ui fields
    accountText: string;
    statusText: string;
    color?: string;
    contrastColor?: string;
    holidays: RequestedHoliday[];
}
