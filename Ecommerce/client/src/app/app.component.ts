import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { IProduct } from './shared/models/product';
import { IPagination } from './shared/models/pagination';
import { NavbarComponent } from './core/navbar/navbar.component';
import { HomeModule } from './home/home.module';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { UnAuthenticatedComponent } from './core/un-authenticated/un-authenticated.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { HeaderComponent } from './core/header/header.component';
import { BreadcrumbComponent} from 'xng-breadcrumb';
import { NgxSpinnerModule } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  imports: [
    HttpClientModule,
    CommonModule,
    NavbarComponent,    
    HomeModule,
    RouterOutlet,
    NotFoundComponent,
    UnAuthenticatedComponent,
    ServerErrorComponent,
    HeaderComponent,
    BreadcrumbComponent,
    NgxSpinnerModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})


export class AppComponent implements OnInit{
  title = 'Vatsy';
  products: IProduct[] = []

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get<IPagination<IProduct[]>>('https://localhost:8010/Catalog/GetAllProducts').subscribe({
      next:response => {
        this.products = response.data,
        console.log(response)
      },
      error: error => console.log(error),
      complete:() => {
        console.log('Catalog API call completed');
      }
    });
  }
}