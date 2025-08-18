import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { error } from 'console';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signin-redirect-callback',
  imports: [CommonModule],
  template: '<div></div>'
  })
export class SigninRedirectCallbackComponent implements OnInit {

    constructor(private _router : Router, private actService : AccountService, private activatedRoute : ActivatedRoute) {

    }

    ngOnInit(): void {
      this.actService.finishLogin()
      .then(_ => {
        if(_){ // user connected
          this._router.navigate(['/checkout'], {replaceUrl: true});
          console.log('user connected', _);
        }else{ 
          console.log('user not connected');
        }
      })
    }
}
