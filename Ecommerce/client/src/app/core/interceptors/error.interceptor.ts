import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpInterceptorFn,
  HttpErrorResponse,
  HttpHandlerFn
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';



export const errorInterceptorFn: HttpInterceptorFn = (request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {

  const router = new Router();
  
  return next(request).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('ERROR from interceptor (functional):', error);
        if(error){
          if(error.status === 404) { router.navigateByUrl('/not-found'); }
          if(error.status === 401)  {router.navigateByUrl('/un-authenticated'); }
          if(error.status === 500)  {router.navigateByUrl('/server-error'); } 
          }
          return throwError(() => error);
      })
    );
  };
