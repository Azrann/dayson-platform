import { IUserId } from '../user/user-id';
import { ICompanyId } from './company-id';

export interface IWebCompanyOverview {
    id: ICompanyId;
    ownerId: IUserId;

    name: string;
}
