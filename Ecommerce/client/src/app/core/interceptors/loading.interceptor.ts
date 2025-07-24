import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { delay, finalize, Observable } from 'rxjs';
import { LoadingService } from '../services/loading.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { inject } from '@angular/core';

export const loadingInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
 
  const loadingService = inject(LoadingService); //Singleton service
  loadingService.loading();
  
  return next(req).pipe(
    delay(1000),
    finalize(() => {
      loadingService.idle();
    })
  );
};
