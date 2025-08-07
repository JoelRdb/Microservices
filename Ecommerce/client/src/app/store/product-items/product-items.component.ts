import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { RouterModule } from '@angular/router';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-items',
  imports: [CommonModule, RouterModule],
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.scss'
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor(private serviceBasket: BasketService) {}

  addItemToBasket(){
    // Si la valeur à gauche est null ou undefined, false , 0 , "" alors l'expression s'arrete et ne va pas exécuter la partie droite.
    // Sinon, si la valeur exist et n'est pas vide, alors elle exécute la partie droite.
    this.product && this.serviceBasket.addItemToBasket(this.product); 
    console.log("ok", this.product?.name);
  }
}
