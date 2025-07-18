import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination'; // Importing PaginationModule for pagination functionality
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot()  // Importing PaginationModule for pagination functionality    
  ]
})
export class StoreModule { }
