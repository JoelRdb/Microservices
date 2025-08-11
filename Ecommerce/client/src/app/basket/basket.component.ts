import { Component, NgModule } from '@angular/core';
import { BasketService } from './basket.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderSummaryComponent } from '../shared/order-summary/order-summary.component';
import { IBasket, IBasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  imports: [
    CommonModule,
    RouterModule,
    OrderSummaryComponent
  ],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss'
})
export class BasketComponent {

  constructor(public basketService: BasketService) {
  }

  incrementItem(item: IBasketItem){
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem){
    this.basketService.decrementItemQuantity(item);
  }

  removeItemFromBasket(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);      
  }
}
