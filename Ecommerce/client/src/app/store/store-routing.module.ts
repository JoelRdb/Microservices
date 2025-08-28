import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import path from 'path';
import { StoreComponent } from './store.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductItemsComponent } from './product-items/product-items.component';



const routes: Routes = [
  // Define your routes here
  {
    path: '', // This path is relative to '/store' (so it matches /store)
    component: StoreComponent,
    data: { breadcrumb: 'Store' } // This breadcrumb is for the main /store page
  },
  {
    path: ':id', // This path is relative to '/store' (so it matches /store/:id)
    component: ProductDetailsComponent,
    data: { 
      breadcrumb: {alias: 'productDetails'}     
    }, // 'productDetails' is the same name as used in the breadcrumb service
  },
  {
    path: 'items', // Example if you have a route for ProductItemsComponent
    component: ProductItemsComponent,
    data: { breadcrumb: 'Articles' }
  }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreComponent,
    ProductDetailsComponent,
    ProductItemsComponent
  ],
  exports: [RouterModule]
})
export class StoreRoutingModule { }
