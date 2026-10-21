import { ITeam } from './ITeam';
import { ICompany } from '../../company/data/ICompany';
import { RequestedHoliday } from '../../holidays/data/requested-holiday';

export interface IDoNotUseForNowTeamUser {
    id: string;
    firstname: string;
    middlename: string;
    lastname: string;
    dateOfBirth: string;
    phoneNumber: string;
    emailAddress: string;
    jobTitle: string;
    role: string;
    pictureUrl: string;
    otherJson: string;

    company: ICompany;
    myTeam: ITeam;

    ////////////////////////////////////////////////////
    // ui calculated fields

    // color for representing holidays
    color?: string;
    contrastColor?: string;
    holidays: RequestedHoliday[];
}
