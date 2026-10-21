import { RequestedHoliday } from '../../holidays/data/requested-holiday';
import { IUserOverview } from '../../user/data/IUserOverview';

export interface IUserBasicInfo {
    id: string;
    firstname: string;
    middlename: string;
    dateOfBirth: string;
    lastname: string;
    phoneNumber: string;
    emailAddress: string;
    jobTitle: string;
    role: string;
    pictureUrl: string;
    otherJson: string;

    company: IShortCompany;

    myTeam: IShortTeam;

    managerOf: IShortUser[];


    ////////////////////////////////////////////////////
    // ui calculated fields

    // color for representing holidays
    holidays: RequestedHoliday[];
    initials: string;
    loading: boolean;
}

export interface IShortTeam {
    id: string;
    name: string;
}

export interface IShortCompany {
    id: string;
    name: string;
}

export interface IShortUser {
    id: string;
    name: string;
}
