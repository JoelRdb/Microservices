import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './core/navbar/navbar.component';
import { HomeModule } from './home/home.module';
import { HeaderComponent } from './core/header/header.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  imports: [
    CommonModule,
    NavbarComponent,    
    HomeModule,
    RouterOutlet,
    HeaderComponent,
    NgxSpinnerModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})


export class AppComponent implements OnInit{
  title = 'Vatsy';

  constructor(private basketService: BasketService) {
  }

  ngOnInit(): void {
    const basket_username = localStorage.getItem('basket_username');
    if(basket_username){
      this.basketService.getBasket(basket_username);
    }
  }
}