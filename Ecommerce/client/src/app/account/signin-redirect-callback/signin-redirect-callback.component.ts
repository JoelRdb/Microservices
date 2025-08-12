import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-signin-redirect-callback',
  imports: [],
  template: '<div></div>'
  })
export class SigninRedirectCallbackComponent implements OnInit {

    constructor(private _router : Router, private actService : AccountService, private activatedRoute : ActivatedRoute) {

    }

    ngOnInit(): void {
      this.actService.finishLogin().then(_ => {
        console.log('inside finish login');
        this._router.navigate(['/checkout'], {replaceUrl: true});
      })
    }
}
