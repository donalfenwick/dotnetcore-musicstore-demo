import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User, OidcClient, MetadataService } from 'oidc-client';
import { Observable } from 'rxjs';


import { ConfiguredUserManager } from '../auth/ConfiguredUserManager';



@Injectable()
export class AuthService {

  user: User;

  constructor(public manager: UserManager) {

    // triggered when the auto renew access_token fails
    this.manager.events.addSilentRenewError(function(){

   });
   
  }
  
  // called when the application starts up to load the user token from the local store
  initAuth(): Promise<void>{
    return new Promise<void>((resolveInitAuth, rejectInitAuth) => {
      this.manager.getUser().then(user => {
        this.user = user;
        resolveInitAuth();
      }, (err) => {
        rejectInitAuth(err);
      });
    });
  }

  getOidcConfig(): Promise<any>{
    return new Promise<any>((resolveFn, rejectFn) => {
      if(this.manager['metadataService']){
          // es6 getter metadataService is not exposed via typescript. cast the type to access
        const metadataService = (<MetadataService>(<any>this.manager).metadataService);
        metadataService.getMetadata().then(configMetadata =>{
          resolveFn(configMetadata);
        }, err => {
          rejectFn(err);
        });
      }else{
        resolveFn(null);
      } 
    });
  }

  isLoggedIn(): boolean {
    let result =  this.user != null && !this.user.expired;
    return result;
  }

  getClaims(): any {
    return this.user.profile;
  }

  getAccessToken(): string {
    if(this.user && this.user.access_token){
       return this.user.access_token;
    }else{
      return '';
    }
  }

  getAuthorizationHeaderValue(): string {
    if(this.user && this.user.token_type && this.user.access_token){
      return `${this.user.token_type} ${this.user.access_token}`;
    }else{
      return '';
    }
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
    });
  }
}
