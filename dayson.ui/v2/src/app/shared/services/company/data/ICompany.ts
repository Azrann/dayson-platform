import { IGlobalCompanyRules } from './IGlobalCompanyRules';

export interface ICompany extends IGlobalCompanyRules {
    companyId: string;
    name: string;
}
