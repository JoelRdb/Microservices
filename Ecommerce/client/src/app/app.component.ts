import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';

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
  products: any[] = []

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get('https://localhost:8010/Catalog/GetProductByBrandName/Adidas').subscribe({
      next:(response:any) => {
        this.products = response,
        console.log(response)
      },
      error: error => console.log(error),
      complete:() => {
        console.log('Catalog API call completed');
      }
    });
  }
}