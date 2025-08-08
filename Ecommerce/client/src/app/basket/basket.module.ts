import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketRoutingModule } from './basket-routing.module';
import { OrderSummaryComponent } from '../shared/order-summary/order-summary.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule, 
    BasketRoutingModule,
    OrderSummaryComponent
  ]
})
export class BasketModule { }
