import { Component, OnInit } from '@angular/core';
import { StoreService } from './store.service';
import { IProduct } from '../shared/models/product';
import { CommonModule } from '@angular/common';
import { ProductItemsComponent } from './product-items/product-items.component';
import { types } from 'util';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';

@Component({
  selector: 'app-store',
  imports: [
    CommonModule,
    ProductItemsComponent,

  ],
  templateUrl: './store.component.html',
  styleUrl: './store.component.scss'
})
export class StoreComponent implements OnInit {
  /**
   *
   */
  products: IProduct[] = [] ;
  brands: IBrand[] = [];
  types: IType[] = [];

  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
  
  }
  
  getProducts(){
    this.storeService.getProducts().subscribe({
        next: response => this.products = response.data,
        error: error => console.log(error),
    });
  }

  getBrands(){
    this.storeService.getBrands().subscribe({
        next: response => this.brands = response,
        error: error => console.log(error),
    });
  }  
  getTypes(){
    this.storeService.getTypes().subscribe({
        next: response => this.types = response,
        error: error => console.log(error),
    });
  }

}
