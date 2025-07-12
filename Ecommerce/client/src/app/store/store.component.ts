import { Component, OnInit } from '@angular/core';
import { StoreService } from './store.service';
import { IProduct } from '../shared/models/product';
import { CommonModule } from '@angular/common';
import { ProductItemsComponent } from './product-items/product-items.component';

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
  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
    this.storeService.getProducts().subscribe({
      next: response => this.products = response.data,
      error: error => console.log(error),
    });
  }
  
  
}
