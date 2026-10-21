import { IApprover } from '../../user/data/IApprover';

export interface IGlobalCompanyRules {
    name: string;

    defaultNumberOfHolidays: number;
    maxConsecutiveDaysOff: number;
    minDaysAdvanceNoticeHolidays: number;
    maxDaysCarriedOverNextYear: number;
    yearStartDay: Date;
    companyId: string;

    workingDaysOfTheWeek: {
        monday: boolean;
        tuesday: boolean;
        wednesday: boolean;
        thursday: boolean;
        friday: boolean;
        saturday: boolean;
        sunday: boolean;
    };

    // not in specification
    // http://dayson-facade.gear.host/swagger/ui/index#!/Company/Company_Create
    countryId: string;
    openDuringBankHolidays: boolean;
    sizeId: number;

    initialized: boolean;
}
