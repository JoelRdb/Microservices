import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { Constants } from './constants';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  // Nous avons besoin d'un élément qui n'émette pas de valeur initiale, mais qui attende d'avoir quelque chose à émettre.
  // C'est pourquoi j'ai utilisé Replaysubject. Je lui ai demandé de conserver un objet utilisateur et il le mettra également en cache.
  // ReplaySubject<any>(1) → garde en mémoire uniquement la dernière valeur émise.
  // Quand un nouvel abonné s’abonne → il reçoit cette dernière valeur immédiatement.
  // Si on mettait ReplaySubject<any>(3), il recevrait les 3 dernières valeurs dans l’ordre où elles ont été émises.
  // BehaviorSubject doit toujours avoir une valeur initiale (et émet cette valeur même avant le premier .next()).
  // ReplaySubject(1) n’émet rien tant qu'on n’as pas appelé .next() au moins une fois.
  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();
  private manager = new UserManager(getClientSettings());
  private user!: User | null;
  token = "";
  access_token = "";

  constructor(private router: Router) {
    this.manager.getUser().then(user => {
      this.user = user,
      this.currentUserSource.next(this.isAuthenticated());
    });
  }

  isAuthenticated(): boolean{
    return this.user != null && !this.user.expired;
  }

  login(){
    return this.manager.signinRedirect();
  }

  async signout(){
    await this.manager.signoutRedirect();
  }

get authorizationHeaderValue() : string {
  console.log(this.token);
  console.log(this.access_token);
  return '${this.token} ${this.access_token}';
}

  logout()
  {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  public finishLogin = (): Promise<User> => {
    return this.manager.signinRedirectCallback().then(user => {
      this.currentUserSource.next(this.checkUser(user));
      this.token = user.token_type;
      this.access_token = user.access_token;
      return user;
    })
  }
  checkUser(user: User): boolean {
    console.log('inside check user');
    console.log(user);
    return !!user && !user.expired;
  }


  public finishLogout = () => {
    this.user = null;
    return this.manager.signoutRedirectCallback();
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    includeIdTokenInSilentRenew: true,
    automaticSilentRenew: true,
    silent_redirect_uri: '${Constants.clientRoot}/silent-callback.html',
    authority: Constants.idpAuthority,
    client_id: Constants.clientId,
    redirect_uri: '${Constants.clientRoot}/signin-callback',
    scope: "openid profile ecommercegateway",
    response_type: "code",
    post_logout_redirect_uri: '${Constants.clientRoot}/signout-callback'
  };
}