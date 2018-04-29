import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthGuardService implements CanActivate {

  //guard that can be applied to particular routes to prevent unauthenticated users browsing to them

  constructor(private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean{
    if(this.authService.isLoggedIn()){
      return true;
    }
    this.authService.startAuthentication();
    return false;
  }
  

}
