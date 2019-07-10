import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpResponse,
    HttpHandler,
    HttpEvent,
    HttpErrorResponse
} from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';


@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token: string = this.authService && this.authService.user ? this.authService.user.idToken : null;
        // console.log('HttpConfigInterceptor', token, this.authService.user);
        if (token && !request.headers.has('Authorization')) {
            request = request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + token) });
            if (this.authService.user.provider) {
              request = request.clone({ headers: request.headers.set('Provider', this.authService.user.provider.toLowerCase()) });
            }
        }

        if (!request.headers.has('Content-Type')) {
            request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
        }

        request = request.clone({ headers: request.headers.set('Accept', 'application/json') });

        /* Enable this for request interception and error handling */
        return next.handle(request).pipe(
            map((event: HttpEvent<any>) => {
                // if (event instanceof HttpResponse) {
                    // console.log('HttpConfigInterceptor', request, event, token);
                    // this.errorDialogService.openDialog(event);
                // }
                return event;
            }),
                catchError((error: HttpErrorResponse) => {
                let data = {};
                data = {
                    reason: error && error.error && error.error.reason ? error.error.reason : '',
                    status: error.status
                };
                // this.errorDialogService.openDialog(data);
                if(error.status !== 404) {
                  console.log('HttpConfigInterceptor', 'error', error, data, request);
                }
                return throwError(error);
            }));
    }
}
