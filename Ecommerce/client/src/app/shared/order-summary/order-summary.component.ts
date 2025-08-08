import { Component } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-summary',
  imports: [
    CommonModule,
  ],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {

  constructor(public basketService: BasketService) {
    
  }

}
