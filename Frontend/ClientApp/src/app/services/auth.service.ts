import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User, OidcClient } from 'oidc-client';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/delay';
import { ConfiguredUserManager } from '../auth/ConfiguredUserManager';



@Injectable()
export class AuthService {

  user: User;
  isAuthenticated: boolean;
  constructor(public manager: UserManager) {
    this.manager.getUser().then(user => {
      this.user = user;
      this.isAuthenticated = this.isLoggedIn();
    });

    // triggered when the auto renew access_token fails
    this.manager.events.addSilentRenewError(function(){
      if(this){
        this.isAuthenticated = false;
        //this.getUser().
      }
   });
   
  }
   
  isLoggedIn(): boolean {
    let result =  this.user != null && !this.user.expired;
    this.isAuthenticated = result;
    return result;
  }
  getClaims(): any {
    return this.user.profile;
  }
  getAccessToken(): string {
    return this.user ? this.user.access_token : "";
  }
  getAuthorizationHeaderValue(): string {
    return `${this.user ? this.user.token_type : ''} ${this.user ? this.user.access_token : ''}`;
  }
  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  startSignout(): Promise<void>{
    return this.manager.signoutRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
        this.user = user;
        this.isAuthenticated = this.isLoggedIn();
        console.log('completeAuthentication() user:', this.user);
    });
  }
}
