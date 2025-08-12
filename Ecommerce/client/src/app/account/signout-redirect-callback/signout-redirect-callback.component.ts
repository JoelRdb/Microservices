import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signout-redirect-callback',
  imports: [],
  template: '<div></div>'
})
export class SignoutRedirectCallbackComponent implements OnInit{
  /**
   *
   */
  constructor(private _router: Router, private acntService: AccountService) {
    
  }
  ngOnInit(): void {
    this.acntService.finishLogout().then(_ => {
      this._router.navigate(['/'], { replaceUrl: true} );
    })
  }


}
