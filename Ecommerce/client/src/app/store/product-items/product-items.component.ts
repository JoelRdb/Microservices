import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-items',
  imports: [CommonModule, RouterModule],
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.scss'
})
export class ProductItemsComponent {
  @Input() product?: IProduct;
}
