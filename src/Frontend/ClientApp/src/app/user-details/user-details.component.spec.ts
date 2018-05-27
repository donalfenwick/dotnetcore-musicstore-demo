import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDetailsComponent } from './user-details.component';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { MusicstoreService } from '../services/musicstore.service';
import { UserManager } from 'oidc-client';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UserProfile } from '../models/usermodels';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import { By } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { of, empty } from 'rxjs';

describe('UserDetailsComponent', () => {
  let component: UserDetailsComponent;
  let fixture: ComponentFixture<UserDetailsComponent>;


  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[
        FormsModule,
        ReactiveFormsModule,
        NgbModule.forRoot()
        ],
      declarations: [ UserDetailsComponent ],
      providers:[
        { provide: AuthService, useValue: new AuthServiceMock(null) },
        { provide: MusicstoreService, useValue: new MusicstoreService(null, null) }
      ]
    });
    var profileStub: UserProfile = {
      username: '',
      firstname: '',
      surname: '',
      emailAddress: '',
      phoneNumber: '',
      dateOfBirth: ''
    };

    fixture = TestBed.createComponent(UserDetailsComponent);
    component = fixture.componentInstance;
    
    spyOn(TestBed.get(AuthService), 'getClaims').and.returnValue({}); 
    spyOn(TestBed.get(MusicstoreService), 'getUserProfile').and.returnValue(of(profileStub));
    fixture.detectChanges();
  });

  afterEach(() => {
    fixture.destroy();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set values on the profile from the form when submited', () => {
    var spy = spyOn(TestBed.get(MusicstoreService),'updateUserProfile').and.returnValue(of({}));
    component.profileForm.controls['username'].setValue('testusernmae');
    component.profileForm.controls['emailaddress'].setValue('testuser@test.com');
    component.profileForm.controls['firstname'].setValue('testfirstname');
    component.profileForm.controls['surname'].setValue('testsurname');
    component.profileForm.controls['phonenumber'].setValue('0123456789');
    component.profileForm.controls['dateofbirth'].setValue({year: 2018, month: 1, day: 1});
    
    component.profileForm.updateValueAndValidity();
    component.onProfileFormSubmit();

    expect(component.profileForm.valid).toBe(true);

    expect(component.profile.username).toBe('testusernmae');
    expect(component.profile.emailAddress).toBe('testuser@test.com');
    expect(component.profile.firstname).toBe('testfirstname');
    expect(component.profile.surname).toBe('testsurname');
    expect(component.profile.phoneNumber).toBe('0123456789');
    expect(component.profile.dateOfBirth).toBe('2018-01-01');
  });

  it('formControl.firstname should have required validation error when set to empty value', () =>{
    component.profileForm.controls.firstname.setValue('');

    expect(component.profileForm.controls['firstname'].getError('required')).toBeTruthy();
  });

  it('formControl.firstname should NOT have required validation error when set to non-empty value', () =>{
    component.profileForm.controls.firstname.setValue('samplevalue');

    expect(component.profileForm.controls['firstname'].getError('required')).toBeFalsy();
  });

  it('formControl.phonenumber should have required validation error when set to empty value', () =>{
    component.profileForm.controls.phonenumber.setValue('');

    expect(component.profileForm.controls['phonenumber'].getError('required')).toBeTruthy();
  });

  it('formControl.phonenumber should have minLength validation error when set to less than 10 chars', () =>{
    component.profileForm.controls.phonenumber.setValue('123456789');
 
    expect(component.profileForm.controls['phonenumber'].getError('minlength')).toBeTruthy();
  });

  it('formControl.phonenumber should have pattern validation error when non numeriic chars are supplied', () =>{
    component.profileForm.controls.phonenumber.setValue('abcd123456789'); 

    expect(component.profileForm.controls['phonenumber'].getError('pattern')).toBeTruthy();
  });

  it('firstname required validator invalid when empty', () =>{
    component.profileForm.controls.firstname.setValue('');

    expect(component.profileForm.controls['firstname'].getError('required')).toBeTruthy();
  });

  it('should set values on the formControl object when modfied in the UI form element', () => {
    
    updateInputElement('.firstname-group input', 'firstnameValue');
    updateInputElement('.surname-group input', 'surnameValue')
    updateInputElement('.phonenum-group input', 'phonenumValue')
    updateInputElement('.dob-group input', '2018-01-01');

    expect(component.profileForm.controls['firstname'].value).toBe('firstnameValue');
    expect(component.profileForm.controls['surname'].value).toBe('surnameValue');
    expect(component.profileForm.controls['phonenumber'].value).toBe('phonenumValue');
    expect(component.profileForm.controls['dateofbirth'].value).toEqual({year: 2018, month: 1, day: 1});
  });

  it('submit button should be disabled between the time when the onProfileFormSubmit function is called and the MusicService.updateUserProfile observable returns a value', () => {
    spyOnProperty(component.profileForm, 'valid').and.returnValue(true);
    var spy = spyOn(TestBed.get(MusicstoreService),'updateUserProfile').and.returnValue(empty()); // return empty to prevent calling subscribe() in the method
    const el: HTMLButtonElement = fixture.debugElement.query(By.css('.submit-form-btn')).nativeElement;
    
    component.onProfileFormSubmit();
    fixture.detectChanges();
    
    expect(component.savingInProgress).toBe(true);
    expect(el.disabled).toBe(true);

  });


  // utility function that updates the value of an input element in the dom and triggers the change so its available to angular
  function updateInputElement(elementCssSelelctor:string, newValue: any){
    const el: HTMLInputElement = fixture.debugElement.query(By.css(elementCssSelelctor)).nativeElement;
    el.value = newValue;
    el.dispatchEvent(new Event('input'));
    fixture.detectChanges();
    return fixture.whenStable();
  }
});
