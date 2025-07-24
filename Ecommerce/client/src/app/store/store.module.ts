import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination'; // Importing PaginationModule for pagination functionality
import { RouterModule } from '@angular/router';
import { StoreRoutingModule } from './store-routing.module';
import { StoreComponent } from './store.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductItemsComponent } from './product-items/product-items.component';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),  // Importing PaginationModule for pagination functionality
    StoreRoutingModule
  ]
})
export class StoreModule { }
