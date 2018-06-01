import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';

@Injectable()
export class AppInitService {

  constructor(private authService:AuthService) { }

  // called when app is starting, place any required async calls here
  initApp(): Promise<any>{

    // pre-fetch the oidcConfig from identity server so ew only need to make a single ajax call when the user clicks login instead of two
    this.authService.getOidcConfig()
      .then( oidcConfig => { })
      .catch( err =>{ console.log('error loading oidc config at app startup', err); });

    return new Promise( (resolveFn, rejectFn) =>{
      // load the user auth info before the app starts so we dont redirect out to identy server evey time
      this.authService.initAuth().then(resolveFn, rejectFn);
    });
  }
}
