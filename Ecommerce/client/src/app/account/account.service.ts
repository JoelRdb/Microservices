import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ReplaySubject } from 'rxjs';
import { User, UserManager, UserManagerSettings, WebStorageStateStore } from 'oidc-client';
import { Constants } from './constants';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  // Initialisation du manager à null
  private manager: UserManager | undefined;
  private user!: User | null;
  token = "";
  access_token = "";

  constructor(
    private router: Router, 
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // 1. Vérifier si l'application s'exécute dans un navigateur
    if (isPlatformBrowser(this.platformId)) {
        this.manager = new UserManager(getClientSettings());
        this.manager.getUser().then(user => {
            this.user = user,
            this.currentUserSource.next(user);
        });
    }
  }
  

  // 2. Mettre à jour les autres méthodes pour vérifier le manager
  // Vérifie qu'on est côté navigateur avant de renvoyer UserManager
  private getUserManager(): UserManager | undefined {
    if (!isPlatformBrowser(this.platformId)) {
      console.warn('UserManager non disponible côté serveur');
      return undefined;
    }
    return this.manager;
  }


  isAuthenticated(): boolean {
    const userManager = this.getUserManager();
    return this.user != null && !this.user.expired;
  }

  login() {
    const userManager = this.getUserManager();
    if(!userManager){
      throw new Error('Useranager is not initialized.')
    }
    return userManager.signinRedirect();
  }
  
  async signout() {
    const userManager = this.getUserManager();
    if(!userManager){
      throw new Error('Useranager is not initialized.')
    }
    await userManager.signoutRedirect();
  }

  get authorizationHeaderValue(): string {
    console.log(this.token);
    console.log(this.access_token);
    return `${this.token} ${this.access_token}`;
  }

  logout() {
    if(isPlatformBrowser(this.platformId)){
      localStorage.removeItem('token');
      this.currentUserSource.next(null);
      this.router.navigateByUrl('/');
    }    
  }

// finishLogin côté navigateur uniquement
  public finishLogin(): Promise<User | null> {
    const userManager = this.getUserManager();
    if (!userManager) return Promise.resolve(null);

    return userManager.signinRedirectCallback().then(user => {
      if (user) {
        this.currentUserSource.next(user);
      }
      return user;
    });
  }

// Véfiei que l'utilisateur existe et son token n'est pas expiré
  checkUser(user: User): boolean {
    console.log('inside check user');
    console.log(user);
    return !!user && !user.expired;
  }


  public finishLogout = () => {
    this.user = null;
    const userManager = this.getUserManager();
    if(!userManager){
      throw new Error('Useranager is not initialized.')
    }
    return userManager.signoutRedirectCallback();
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    includeIdTokenInSilentRenew: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${Constants.clientRoot}/silent-callback.html`,
    authority: Constants.idpAuthority,
    client_id: Constants.clientId,
    redirect_uri: `${Constants.clientRoot}/signin-callback`,
    scope: "openid profile ecommerceangular roles",
    response_type: "code",
    post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`
  };
}