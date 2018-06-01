import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navmenu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.less']
})
export class NavmenuComponent implements OnInit {

  constructor(public authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  loginInProgress: boolean = false;
  searchQuery: string;

  loginButtonOnClick():void{
    
    // if the user is not already logged in redirect them out to the identity server
    if(!this.authService.isLoggedIn() && !this.loginInProgress){
      this.loginInProgress = true;
      this.authService.startAuthentication()
        .catch(err => {
          this.loginInProgress = false;
          console.log('error redirecting user at login', err);
        })
        .then(() => {
          // no action, user will be refirected by the service
        });
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
