import { Component } from '@angular/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  title = "Login";

  constructor(private actService : AccountService) {
    
  }

  login(){
    this.actService.login();
  }
}
