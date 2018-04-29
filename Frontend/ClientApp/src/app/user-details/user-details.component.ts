import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { MusicstoreService } from '../services/musicstore.service';
import { UserProfile } from '../models/usermodels';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {

  constructor(private authService: AuthService, private service: MusicstoreService) { }

  claimsPrinicpal: any;
  profile: UserProfile;
  savingInProgress: boolean = false;
  
  ngOnInit() {
    this.claimsPrinicpal = this.authService.getClaims();

    this.service.getUserProfile().subscribe( result => {
      this.profile = result;
    })
  }

  onUpdateProfileBtnClick(): void{
    if(this.profile !== null){
      this.savingInProgress = true;
      this.service.updateUserProfile(this.profile)
        .subscribe(result => {
          this.profile = result;
          this.savingInProgress = false;
        });
    }
  }
}
