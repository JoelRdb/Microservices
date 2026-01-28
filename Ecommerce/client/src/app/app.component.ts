import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
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

  constructor(private basketService: BasketService,
    @Inject(PLATFORM_ID) private platformId: Object) {
  }

  ngOnInit(): void {
    if(isPlatformBrowser(this.platformId)){ // Vérifie qu'on est bien dans le navigateur
      const basket_username = localStorage.getItem('basket_username');
          if(basket_username){
            this.basketService.getBasket(basket_username);
          }
    }    
  }
}