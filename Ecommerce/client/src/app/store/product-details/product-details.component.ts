import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { response } from 'express';
import { CommonModule } from '@angular/common';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  imports: [CommonModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  product? : IProduct;

  constructor(private storeService : StoreService, private activateRoute: ActivatedRoute, private bcService: BreadcrumbService) {
    // Set the breadcrumb for this component

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
}
