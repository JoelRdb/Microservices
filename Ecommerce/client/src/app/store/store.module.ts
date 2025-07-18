import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination'; // Importing PaginationModule for pagination functionality
import { RouterModule } from '@angular/router';
import { StoreRoutingModule } from './store-routing.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),  // Importing PaginationModule for pagination functionality
    StoreRoutingModule
  ]
})
export class StoreModule { }
