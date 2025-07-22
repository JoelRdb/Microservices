import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';

/**
 *
 */
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  
  constructor(private router : Router) { }

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error) => {
        if(error){
          if(error.status === 401) {
            this.router.navigateByUrl('/unauthorized');
          }
          if(error.status === 404) {
            this.router.navigateByUrl('/not-found');
          }
          if(error.status === 500) {
            this.router.navigateByUrl('/server-error');
          }
        }
        return throwError(() => new Error(error));
      })
    );
  }
  } 