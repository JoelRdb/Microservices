import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BreadcrumbComponent, BreadcrumbService} from 'xng-breadcrumb';

@Component({
  selector: 'app-header',
  imports: [
    BreadcrumbComponent, 
    CommonModule
],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  constructor(public bcService: BreadcrumbService) {
    // Set the breadcrumb for the header component

    
  }
}
