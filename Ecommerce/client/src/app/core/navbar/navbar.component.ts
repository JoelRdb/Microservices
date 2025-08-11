import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BasketService } from '../../basket/basket.service';
import { IBasketItem } from '../../shared/models/basket';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent  {

  constructor(public basketService : BasketService) {}

  getBasketCount(items: IBasketItem[]){
    return items.reduce((sum, items)=>sum + items.quantity, 0)
  }

}
