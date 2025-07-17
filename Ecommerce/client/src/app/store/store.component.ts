import { Component, OnInit } from '@angular/core';
import { StoreService } from './store.service';
import { IProduct } from '../shared/models/product';
import { CommonModule } from '@angular/common';
import { ProductItemsComponent } from './product-items/product-items.component';
import { types } from 'util';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';
import { StoreParams } from '../shared/models/storeParams';
import { PaginationComponent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-store',
  imports: [
    CommonModule,
    ProductItemsComponent,
    PaginationComponent,
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
  storeParams = new StoreParams();
  setOptions = [
    {name:"Alphabetique", value:"name"},
    {name:"Prix croissant", value:"priceAsc"},  
    {name:"Prix decroissant", value:"priceDesc"},
  ]
  totalCount = 0;

  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands(); 
    this.getTypes();
  }
  
  getProducts(){
    this.storeService.getProducts(this.storeParams).subscribe({
        next: response => {
          this.products = response.data;
          this.storeParams.pageNumber = response.pageIndex;
          this.storeParams.pageSize = response.pageSize;
          this.totalCount = response.count;
        },
        error: error => console.log(error),
    });
  }

  getBrands(){
    this.storeService.getBrands().subscribe({
        next: response => 
          this.brands = [{id:'', name: 'All'}, ...response],
        error: error => console.log(error),
    });
  }  
  getTypes(){
    this.storeService.getTypes().subscribe({
        next: response => 
          this.types = [{id:'', name: 'All'}, ...response],
        error: error => console.log(error),
    });
  }

  onBrandSelected(brandId: string){
    this.storeParams.brandId = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: string){
    this.storeParams.typeId = typeId;
    this.getProducts();
  } 

  onSortSelected(sort: string){
    this.storeParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any) {
    if (this.storeParams.pageNumber = event.page) {
      this.getProducts();
    }
  }
}
