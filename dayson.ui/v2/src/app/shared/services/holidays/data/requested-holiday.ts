import { HolidayColor } from '../../../../holidays/holidays-year/data/holiday-color';
import { IUserBasicInfo } from '../../auth-user/data/AuthUser';
import { HolidayStatus } from './holiday-status';

export class RequestedHoliday {
    id: string;
    employeeId: string;
    user: IUserBasicInfo;

    from: Date;
    to: Date;
    createdOn: Date;
    lastUpdated: Date;

    status: HolidayStatus;
    numberOfDays: number;
    notes: string;
    year: number;

    // ui properties
    hovered = false;
    index?: number;

    public static fromDto(rh: IRequestedHolidayDto): RequestedHoliday {
        const rho = new RequestedHoliday();

        rho.id = rh.id;
        rho.employeeId = rh.employeeId;

        rho.from = new Date(rh.from);
        rho.to = new Date(rh.to);
        rho.createdOn = new Date(rh.createdOn);
        rho.lastUpdated = new Date(rh.lastUpdated);

        rho.status = rh.status;
        rho.numberOfDays = rh.numberOfDays;
        rho.notes = rh.notes;
        rho.year = rh.year;

        return rho;
    }

    constructor(
        year?: number
    ) {
        this.year = year;
    }
}

export interface IRequestedHolidayDto {
    id: string;
    employeeId: string;

    from: string;
    to: string;
    createdOn: string;
    lastUpdated: string;

    status: HolidayStatus;
    numberOfDays: number;
    notes: string;
    year: number;
}
