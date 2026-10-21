import { IWebCompanyOverview } from '../../../data/company/web-company-overview';

export interface IGetCompanyOverviewRequest {
    _IGetCompanyOverviewRequest?: any;
}

export interface IGetCompanyOverviewResponse {
    company: IWebCompanyOverview;
}
