import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  // component handles users being rdirected back from identity server 
  // and calls to persist their token and then redirects the user to the homepage
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    
    this.authService.completeAuthentication().then(() =>{
        this.router.navigate(['']);
    });
  }

}
