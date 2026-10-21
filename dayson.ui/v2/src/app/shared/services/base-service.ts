import { Http } from '@angular/http';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { AuthHttp } from 'angular2-jwt';


export abstract class BaseService {
    // we will modify this when building and replace it with whatever the service url is
    private static _baseUrl = '@{BASE_URL}';

    constructor(
        protected http: Http,
        protected authHttp: AuthHttp,
    ) {
    }

    protected get<T>(path: string): Observable<T> {
        const url = this.buildUrl(path);

        return this.authHttp
            .get(url)
            .catch(this.handleError)
            .map(v => this.extractData(v));
    }

    protected post<T>(path: string, body: any): Observable<T> {
        const url = this.buildUrl(path);

        return this.authHttp
            .post(url, body)
            .catch(this.handleError)
            .map(v => this.extractData<T>(v));
    }

    protected unauthenticatedPost<T>(path: string, body: any): Observable<T> {
        const url = this.buildUrl(path);

        return this.http
            .post(url, body)
            .catch(this.handleError)
            .map(v => this.extractData<T>(v));
    }

    protected buildUrl(path: string): string {
        let url = BaseService._baseUrl;
        if (url.charAt(0) === '@') {
            url = 'http://dayson-facade.gear.host/v1/';
        }

        url = url + path;

        return url;
    }

    protected extractData<T>(response: Response): T {
        const body: T = response.json();
        return body || null;
    }

    protected handleError(error: Response): Observable<Response> {
        console.error(error);
        return Observable.throw(error);
    }
}
