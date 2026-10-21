import { AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import { Subscription } from 'rxjs/Subscription';

import { IComRequest } from './IComRequest';
import { IComRequestConfig } from './IComRequestConfig';

export abstract class ComService {
    // protected BASEURL = 'http://dayson-facade.gear.host/v1';
    protected BASEURL = 'http://localhost:53465/v1';

    private requestTimeoutCancelToken: { [id: string]: any } = {} = {};
    private httpRequestSubscriptions: { [id: string]: Subscription } = {};
    private requestObservers: { [id: string]: Observer<any>[] } = {};

    private cachedResponses: { [id: string]: any } = {};

    public workingRequestCount = 0;

    /**
     * @param $q angular $q service
     * @param $http angular $http service
     * @param $timeout angular $timeout service
     * @param urlGenerator method that will generate the urls pointing to the specific backend (normally a BaseUrlService method)
     */
    protected constructor(
        protected authHttp: AuthHttp,
    ) {
    }

    /**
     * Performs a request to the backend.
     *
     * This method has 2 was of functioning:
     * 1) direct request
     *    when no operationId is supplied, the request is handled normally without delay and without cancelling
     *    functionality.
     *
     * 2) delayed request
     *    when an operationId is supplied, you can also provide a delay (or use the default one). consecutive
     *    requests with the same id will cancel previous ones (both their timers and their responses).
     *    this is useful for when the user can generate multiple requests in rapid sucession and we are only
     *    interested in the last one.
     *    all the previous ones will be removed.
     *
     * @param method http method to use, POST or GET
     */
    protected request<TRequest extends IComRequest<TResponse>, TResponse, TResult>(
        config: IComRequestConfig<TRequest, TResponse, TResult>,
    ): Observable<TResult> {
        // return a promise
        return new Observable<TResult>((observer: Observer<TResult>) => {
            if (!config.method) {
                config.method = 'GET';
            }

            if (config.operationId) {
                // if this operation is tracked by an id...
                const operationCode = this.generateOperationCode(config.endpoint, config.operationId);
                config.cacheId = config.cacheId || config.operationId;
                config.request = config.request || {} as any;

                if (config.clearExistingCache) {
                    delete this.cachedResponses[config.cacheId];
                }

                if (config.useCaching && this.cachedResponses[config.cacheId]) {
                    observer.next(this.cachedResponses[config.cacheId]);
                    observer.complete();
                    return;
                }

                if (!config.cancelPreviousRequest) {
                    // if we don't want to cancel previous promises we want to resolve all the promises at the same time
                    // so we add them to a shared list where all of them are
                    if (this.requestObservers[operationCode]) {
                        // the lists already exists! we are not the first ones
                        // we just add our promises to the lists
                        this.requestObservers[operationCode].push(observer);

                        // and we return because someone is already doing our job
                        return;
                    } else {
                        // we are the first ones
                        // let's just add the list and the promises
                        this.requestObservers[operationCode] = [observer];

                        // and we continue because we are the first and we have to do it ourselves
                    }
                }

                if (config.cancelPreviousRequest) {
                    // if we are cancelling previous requests
                    // (that means we don't need the results from previous ones)
                    // we just cancel the previous request timeout (in case it wasn't executed yet)
                    if (this.requestTimeoutCancelToken[operationCode]) {
                        clearTimeout(this.requestTimeoutCancelToken[operationCode]);
                        delete this.requestTimeoutCancelToken[operationCode];
                    }

                    // and cancel the request also
                    if (this.httpRequestSubscriptions[operationCode]) {
                        this.httpRequestSubscriptions[operationCode].unsubscribe();
                        delete this.httpRequestSubscriptions[operationCode];
                    }

                    // we add our promises to the list as single elements
                    this.requestObservers[operationCode] = [observer];
                }

                // start the timeout for the request
                this.requestTimeoutCancelToken[operationCode] = setTimeout(() => {
                    delete this.requestTimeoutCancelToken[operationCode];

                    this.workingRequestCount++;
                    this.httpRequestSubscriptions[operationCode] = this.authHttp.request(
                        config.endpoint, {
                            method: config.method === 'POST' ? 'Post' : 'Get',
                            body: config.method === 'POST' ? config.request : undefined,
                            params: config.method === 'GET' ? config.request : undefined,
                        }).subscribe((response) => {
                            delete this.httpRequestSubscriptions[operationCode];
                            this.workingRequestCount--;

                            let result: TResult;

                            let responseJson: any;
                            try {
                                responseJson = response.json();
                            } catch {
                                responseJson = undefined;
                            }

                            if (responseJson !== undefined && config.transformer) {
                                result = config.transformer(responseJson);
                            } else {
                                result = responseJson as any;
                            }

                            // resolve all related promises

                            const observers = this.requestObservers[operationCode];

                            // delete all related promises
                            delete this.requestObservers[operationCode];

                            observers.forEach((obs: Observer<TResult>) => {
                                obs.next(result);
                                obs.complete();
                            });

                            // cache response
                            if (config.useCaching) {
                                this.cachedResponses[config.cacheId] = result;
                            }
                        }, (reason) => {
                            delete this.httpRequestSubscriptions[operationCode];

                            this.workingRequestCount--;
                            if (reason.status !== -1) {
                                const observers = this.requestObservers[operationCode];

                                // delete all related promises
                                delete this.requestObservers[operationCode];

                                // reject all related promises
                                observers.forEach((obs: Observer<TResult>) => {
                                    obs.error({
                                        request: config.request,
                                        response: reason,
                                    });

                                    obs.complete();
                                });
                            }
                        });
                }, config.delay);

            } else {
                this.workingRequestCount++;
                this.authHttp.request(
                    config.endpoint, {
                        method: config.method === 'POST' ? 'Post' : 'Get',
                        body: config.method === 'POST' ? config.request : undefined,
                        params: config.method === 'GET' ? config.request : undefined,
                }).subscribe((response) => {
                    this.workingRequestCount--;

                    let result: TResult;
                    if (config.transformer) {
                        result = config.transformer(response.json());
                    } else {
                        result = response.json() as any;
                    }

                    observer.next(result);
                }, (reason) => {
                    this.workingRequestCount--;

                    observer.error({
                        request: config.request,
                        response: reason,
                    });
                });
            }
        });
    }

    private generateOperationCode(endpoint: string, operationId: string) {
        return `${endpoint}-${operationId}`;
    }

    protected invalidateCache(endpoint?: string, operationId?: string, cacheId?: string) {
        if (!endpoint || !operationId) {
            this.cachedResponses = {};
        } else {
            cacheId = cacheId || this.generateOperationCode(endpoint, operationId);

            delete this.cachedResponses[cacheId];
        }
    }

    /**
     * Cancel single requests (not queues)
     * @param operationId id of the operation to cancel
     */
    protected cancelRequest(endpoint: string, operationId: string) {
        const operationCode = this.generateOperationCode(endpoint, operationId);

        if (this.requestTimeoutCancelToken[operationCode]) {
            clearTimeout(this.requestTimeoutCancelToken[operationCode]);
            delete this.requestTimeoutCancelToken[operationCode];
        }

        // and cancel the request also
        if (this.httpRequestSubscriptions[operationCode]) {
            this.httpRequestSubscriptions[operationCode].unsubscribe();
            delete this.httpRequestSubscriptions[operationCode];
        }

        // we add our promises to the list as single elements
        delete this.requestObservers[operationCode];
    }

    /* convenience method to execute a GET using request */
    protected get<TRequest, TResponse, TResult>(config: IComRequestConfig<TRequest, TResponse, TResult>): Observable<TResult> {
        config.method = 'GET';
        return this.request(config);
    }

    /* convenience method to execute a POST using request */
    protected post<TRequest, TResponse, TResult>(config: IComRequestConfig<TRequest, TResponse, TResult>): Observable<TResult> {
        config.method = 'POST';
        return this.request(config);
    }
}
