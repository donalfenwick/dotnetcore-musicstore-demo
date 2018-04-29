import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-auth-silentrefresh-callback',
  templateUrl: './auth-silentrefresh-callback.component.html',
  styleUrls: ['./auth-silentrefresh-callback.component.css']
})
export class AuthSilentrefreshCallbackComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.manager.signinSilentCallback();
  }

}
