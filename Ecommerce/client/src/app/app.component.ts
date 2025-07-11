import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [
    NavbarComponent,
    HttpClientModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})


export class AppComponent implements OnInit{
  title = 'VATSY MDG';

  constructor(private http: HttpClient) {

  }
  ngOnInit(): void {
    this.http.get('https://id-local.eshopping.com:44344/Catalog/GetProductByBrandName/Adidas').subscribe({
      next:response => console.log(response),
      error: error => console.log(error),
      complete:() => {
        console.log('Catalog API call completed');
      }
    });
  }
}