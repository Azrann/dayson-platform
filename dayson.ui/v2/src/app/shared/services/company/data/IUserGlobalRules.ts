import { IApprover } from '../../user/data/IApprover';

export interface IUserGlobalRules {
    id: string;
    companyId: string;

    name: string;
    email: string;

    holidayEntitlement: number;
    maxDaysCarriedOverNextYear: number;
    minDaysAdvanceNoticeHolidays: number;
    maxConsecutiveDaysOff: number;
    yearStartDay: string;

    workingDaysOfTheWeek: {
        sunday: boolean;
        monday: boolean;
        tuesday: boolean;
        wednesday: boolean;
        thursday: boolean;
        friday: boolean;
        saturday: boolean
    };

    isApprover: boolean;
    approver: IApprover;
}
