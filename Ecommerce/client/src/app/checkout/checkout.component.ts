import { Component, OnInit } from '@angular/core';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { CommonModule } from '@angular/common';
import { BasketService } from '../basket/basket.service';
import { AccountService } from '../account/account.service';
import { IBasket, IBasketItem } from '../shared/models/basket';
import { OrderSummaryComponent } from '../shared/order-summary/order-summary.component';

@Component({
  selector: 'app-checkout',
  standalone : true,
  imports: [
    CommonModule,
    CheckoutRoutingModule, 
    OrderSummaryComponent
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {

  public isUserAuthenticated: boolean = false;

  constructor(public basketService: BasketService, private acntService : AccountService) {
  }

  ngOnInit(): void {
    this.acntService.currentUser$.subscribe({
      next: (res) => {
        this.isUserAuthenticated = res;
        console.log('Utilisateur authentifié', this.isUserAuthenticated);
      }, error:(err) => {
        console.log(`An error occured while setting isUserAuthenticated flag.`);
      }
    })
  }


  removeBasketItem(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);
  }

  incrementItem(item: IBasketItem){
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem){
    this.basketService.decrementItemQuantity(item);
  }

  orderNow(item: IBasket){
    this.basketService.checkoutBasket(item);
  }

}
