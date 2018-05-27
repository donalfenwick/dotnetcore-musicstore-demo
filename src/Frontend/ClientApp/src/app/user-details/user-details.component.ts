import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { MusicstoreService } from '../services/musicstore.service';
import { UserProfile } from '../models/usermodels';
import { FormGroup, FormControl, RequiredValidator, Validators } from '@angular/forms';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {

  constructor(private authService: AuthService, private service: MusicstoreService, private dateParserFormatter: NgbDateParserFormatter) {  }

  claimsPrinicpal: any;
  profile: UserProfile;
  savingInProgress: boolean = false;

  profileForm: FormGroup;
  
  ngOnInit() {

    this.profileForm = new FormGroup({
      username: new FormControl('',[Validators.required]),
      emailaddress: new FormControl('',[Validators.required]),
      firstname: new FormControl('',[Validators.required]),
      surname: new FormControl('',[Validators.required]),
      phonenumber: new FormControl('',[ 
        Validators.required,
        Validators.minLength(10),
        Validators.pattern(new RegExp('^[0-9]+$'))
      ]),
      dateofbirth: new FormControl(null,[Validators.required])
    });

    this.claimsPrinicpal = this.authService.getClaims();

    this.service.getUserProfile().subscribe( result => {
      this.profile = result;
      this.profileForm.controls.username.setValue(this.profile.username);
      this.profileForm.controls.emailaddress.setValue(this.profile.emailAddress);
      this.profileForm.controls.firstname.setValue(this.profile.firstname);
      this.profileForm.controls.surname.setValue(this.profile.surname);
      this.profileForm.controls.phonenumber.setValue(this.profile.phoneNumber);
      if(this.profile.dateOfBirth && this.profile.dateOfBirth.length>0){
        this.profileForm.controls.dateofbirth.setValue(this.dateParserFormatter.parse(this.profile.dateOfBirth));
      }
    });
  }

  onProfileFormSubmit(): Promise<any>{
    return new Promise( (resolve, reject) => {
      
      if(this.profileForm.valid && this.profile !== null){
        this.savingInProgress = true;

        this.profile.username = this.profileForm.get('username').value;
        this.profile.emailAddress = this.profileForm.get('emailaddress').value;
        this.profile.firstname = this.profileForm.get('firstname').value;
        this.profile.surname = this.profileForm.get('surname').value;
        this.profile.phoneNumber = this.profileForm.get('phonenumber').value;
        this.profile.dateOfBirth = this.dateParserFormatter.format(this.profileForm.get('dateofbirth').value);
        
        this.service.updateUserProfile(this.profile)
          .subscribe(result => {
        
            this.savingInProgress = false;
            resolve();
        }, (e) =>{ 
          reject(e);
        });
      }
    });
  }
}

