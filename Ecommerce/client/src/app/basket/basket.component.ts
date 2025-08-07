import { Component, NgModule } from '@angular/core';
import { BasketService } from './basket.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-basket',
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss'
})
export class BasketComponent {
/**
 *
 */
constructor(public basketService: BasketService) {

  
}
}
