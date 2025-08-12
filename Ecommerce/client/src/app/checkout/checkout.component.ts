import { Component } from '@angular/core';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkout',
  imports: [
    CommonModule,
    CheckoutRoutingModule    
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent {

}
