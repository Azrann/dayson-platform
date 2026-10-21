import { IUpdateCompanyRulesRequest, IUpdateCompanyRulesResponse } from './dtos/update-company-rules';
import { IWebCompanyOverview } from '../../data/company/web-company-overview';
import { Injectable } from '@angular/core';
import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';

import { IWebCompanyRules } from '../../data/company/web-company-rules';
import { ComService } from '../infrastructure/com-service';
import { IGetCompanyRulesRequest, IGetCompanyRulesResponse } from './dtos/get-company-rules';
import { IGetCompanyOverviewRequest, IGetCompanyOverviewResponse } from './dtos/get-company-overview';

@Injectable()
export class WebCompanyService extends ComService {
    constructor(
        authHttp: AuthHttp,
    ) {
        super(authHttp);
    }

    public getCompanyRules(): Observable<IWebCompanyRules> {
        return this.get<IGetCompanyRulesRequest, IGetCompanyRulesResponse, IWebCompanyRules>({
            endpoint: `${this.BASEURL}/WebCompany/getCompanyRules`,
            operationId: `getCompanyRules`,
            request: null,
            transformer: (response) => response.rules
        });
    }

    public getCompanyOverview(): Observable<IWebCompanyOverview> {
        return this.get<IGetCompanyOverviewRequest, IGetCompanyOverviewResponse, IWebCompanyOverview>({
            endpoint: `${this.BASEURL}/WebCompany/getCompanyOverview`,
            operationId: `getCompanyOverview`,
            request: null,
            transformer: (response) => response.company
        });
    }

    public updateCompanyRules(
        rules: IWebCompanyRules
    ): Observable<IWebCompanyRules> {
        return this.post<IUpdateCompanyRulesRequest, IUpdateCompanyRulesResponse, IWebCompanyRules>({
            endpoint: `${this.BASEURL}/WebCompany/updateCompanyRules`,
            operationId: `updateCompanyRules`,
            request: { rules },
            transformer: (response) => response.rules
        });
    }

}
