import { IComRequest } from './IComRequest';

export interface IComRequestConfig<TRequest extends IComRequest<TResponse>, TResponse, TResult> {
    // type of http operation
    method?: 'GET' | 'POST';

    // endpoint in this operation will send the request to
    endpoint: string;

    // data to be sent to the server
    request?: TRequest;

    // response transformer
    // will convert the response from the server into the desired result
    transformer?: (response: TResponse) => TResult;

    // used for advanced operation
    // TricomService will use this id to identify simliar operations
    operationId?: string;

    // for fine tuning caching
    // if you for example want to cache different items but them all belong to the same operationId
    // operationId = 'typeahead'
    // cacheId = 'typeahead-kakao'
    // cacheId = 'typeahead-kuglepen'
    // you'll be able to cancel any previous requests but remember results for specific ones.
    cacheId?: string;

    // cancels waiting/executing similiar operations
    // active when operationId provided
    cancelPreviousRequest?: boolean;

    // delay of this operation in ms
    // active when operationId provided
    delay?: number;

    // true to enable caching
    // active when operationId provided
    useCaching?: boolean;

    // true to clear previous cached responses
    // active when operationId provided
    clearExistingCache?: boolean;

    // // // // // // // // // // // // // // // // // // // // // // // // // // // //
    // // // // // // // // // // // // // // // // // // // // // // // // // // // //
    // INTERNAL STUFF // // // // // // // // // // // // // // // // // // // // // //
}
