import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BasketService } from '../../basket/basket.service';
import { IBasketItem } from '../../shared/models/basket';
import { AccountService } from '../../account/account.service';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit  {
  
  public isUserAuthenticated: boolean = false;

  constructor(public basketService : BasketService, private acntService : AccountService) {}

  ngOnInit(): void {
    console.log('current user');
    this.acntService.currentUser$.subscribe({
      next:(res) => {
        this.isUserAuthenticated = res;
        console.log(this.isUserAuthenticated);
      },error: (err) => {
        console.log('An error occured while setting isUserAuthenticated flag');
        }
    })
  }

  getBasketCount(items: IBasketItem[]){
    return items.reduce((sum, items)=>sum + items.quantity, 0)
  }

  public login = () => {
    this.acntService.login();
  }

  public logout = () => {
    this.acntService.signout();
  }

}
