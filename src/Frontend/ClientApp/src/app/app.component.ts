import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: []
})
export class AppComponent {

  public constructor(private authService: AuthService, private router: Router){
    this.environmentName = environment.envName;
  }
  title:string = 'app';
  searchQuery: string;
  environmentName: string;

  loginButtonOnClick():void{
    // if the user is not already logged in redirect them out to the identity server
    if(!this.authService.isLoggedIn()){
      this.authService.startAuthentication();
    }
  }

  logoutButtonOnClick():void{
    // redirect the user out to identity server to destroy their session
    this.authService.startSignout();
  }

  searchButtonOnClick(): void{
    this.router.navigate([`albums/search/${this.searchQuery}`]);
  }
}
