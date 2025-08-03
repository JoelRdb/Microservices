import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBasket } from '../shared/models/basket';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BasketServiceService {

  baseUrl = 'https://localhost:8010';

  constructor(private http: HttpClient) { }

  private basketSource = new BehaviorSubject<IBasket | null>(null);
  basketSource$ = this.basketSource.asObservable();


  getBasket(username: string){
    return this.http.get<IBasket>(this.baseUrl + '/Basket/GetBasket/joel').subscribe({
      next: (basket) => this.basketSource.next(basket)
      });
  }

  
  setBasket(basket: IBasket) {
    return this.http.post<IBasket>(this.baseUrl + '/Basket/CreateBasket', basket).subscribe({
      next: (basket) => this.basketSource.next(basket)
    });
  }

  getCurrentBasket() {
    return this.basketSource.value;
  }
}
