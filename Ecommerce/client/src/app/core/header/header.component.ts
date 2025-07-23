import { Component } from '@angular/core';
import { BreadcrumbComponent} from 'xng-breadcrumb';

@Component({
  selector: 'app-header',
  imports: [BreadcrumbComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

}
