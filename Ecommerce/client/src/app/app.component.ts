import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { IProduct } from './shared/models/product';
import { IPagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    NavbarComponent,
    HttpClientModule,
    CommonModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})


export class AppComponent implements OnInit{
  title = 'VATSY MDG';
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