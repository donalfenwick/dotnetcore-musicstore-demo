import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';

@Injectable()
export class AppInitService {

  constructor(private authService:AuthService) { }

  // called when app is starting, place any required async calls here
  initApp(): Promise<any>{
    return new Promise( (resolveFn, rejectFn) =>{
      // load the user auth info before the app starts so we dont redirect out to identy server evey time
      this.authService.initAuth().then(resolveFn, rejectFn);
    });
  }
}
