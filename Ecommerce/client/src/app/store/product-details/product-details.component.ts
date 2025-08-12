import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { response } from 'express';
import { CommonModule } from '@angular/common';
import { BreadcrumbService } from 'xng-breadcrumb';
import { IBasketItem } from '../../shared/models/basket';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-details',
  imports: [CommonModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  product? : IProduct;
  quantity = 1;

  constructor(private storeService : StoreService, private activateRoute: ActivatedRoute, 
              private bcService: BreadcrumbService, private basketService: BasketService) {
  }
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.activateRoute.snapshot.paramMap.get('id');
    if(id){
      this.storeService.getProduct(id).subscribe({
        next: response => {
          this.product = response,
          this.bcService.set('@productDetails', response.name); // Set the breadcrumb with the product name) 
        },
        error: error => console.error(error),
      });
    }
  }

  addItemToCart(){
    if(this.product){
      this.basketService.addItemToBasket(this.product, this.quantity);
    }
  }

  incrementQuantity(){
    this.quantity++;
  }
  decrementQuantity(){
    if(this.quantity > 1){
      this.quantity--;
    }
  }
}
