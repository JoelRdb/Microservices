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
  public userName : string = '';

  constructor(public basketService : BasketService, private acntService : AccountService) {

  }

  ngOnInit(): void {
    console.log('current user');
    this.acntService.currentUser$.subscribe(user => {
      this.isUserAuthenticated = !!user && !user.expired;
      this.userName = user?.profile.Username || user?.profile.given_name || '';
      console.log('Username: ' + this.userName);
    });

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
