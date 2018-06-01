import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavmenuComponent } from './navmenu.component';
import { AuthService } from '../services/auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { By } from '@angular/platform-browser';


describe('NavmenuComponent', () => {
  let component: NavmenuComponent;
  let fixture: ComponentFixture<NavmenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavmenuComponent ],
      providers:[
        {provide: AuthService, useValue: new AuthServiceMock(null) }
      ],
      imports: [FormsModule, RouterTestingModule.withRoutes([])]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('AuthService.startAuthentication() should be called when loginButtonOnClick() is called and user is NOT authenticated', () => {
    let isAuthCallSpy = spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(false);
    let startAuthenticationSpy = spyOn(TestBed.get(AuthService), 'startAuthentication').and.returnValue(Promise.resolve({}));

    component.loginButtonOnClick();

    expect(startAuthenticationSpy).toHaveBeenCalled();
  });

  it('AuthService.startAuthentication() should NOT be called when logoutButtonOnClick() called', () => {
    let isAuthCallSpy = spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(true);
    let startAuthenticationSpy = spyOn(TestBed.get(AuthService), 'startAuthentication').and.returnValue(Promise.resolve({}));

    component.loginButtonOnClick();

    expect(startAuthenticationSpy).not.toHaveBeenCalled();
  });

  it('AuthService.startSignout() should NOT be called when loginButtonOnClick() is called and user is authenticated', () => {
    let isAuthCallSpy = spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(true);
    let startSignoutSpy = spyOn(TestBed.get(AuthService), 'startSignout');

    component.logoutButtonOnClick();

    expect(startSignoutSpy).toHaveBeenCalled();
  });

  it('should navigate to search route when searchButtonOnClick() is called', () => {
    const router = TestBed.get(Router);
    let spy = spyOn(router, 'navigate');
    component.searchQuery = 'foo';

    component.searchButtonOnClick();

    expect(spy).toHaveBeenCalledWith(['albums/search/foo'])
  });

  it('searchButtonOnClick() should be called when the search button is clicked in the DOM', () => {
    const spy = spyOn(component, 'searchButtonOnClick');
    let e: HTMLButtonElement = fixture.debugElement.query(By.css('.search-btn')).nativeElement;
    
    e.click();

    expect(spy).toHaveBeenCalled();
  });

  it('loginButtonOnClick() should be called when the login link is clicked in the DOM', () => {
    const spy = spyOn(component, 'loginButtonOnClick');
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(false);
    fixture.autoDetectChanges();
    let e: HTMLAnchorElement = fixture.debugElement.query(By.css('.login-link')).nativeElement;
    
    e.click();

    expect(spy).toHaveBeenCalled();
  });

  it('logoutButtonOnClick() should be called when the logout link is clicked in the DOM', () => {
 
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(true);
    const spy = spyOn(component, 'logoutButtonOnClick'); 
    fixture.autoDetectChanges();
    let e: HTMLAnchorElement = fixture.debugElement.query(By.css('.logout-link')).nativeElement;
    
    e.click();

    expect(spy).toHaveBeenCalled();
  });

  it('.logout-link should NOT be present in the components DOM when the user is NOT logged in', () => {
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(false);
    fixture.autoDetectChanges();
    let numElemenets = fixture.debugElement.queryAll(By.css('.logout-link')).length;
    
    expect(numElemenets).toBe(0);
  });


  it('.logout-link should be present in the components DOM when the user is logged in', () => {  
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(true);
    fixture.autoDetectChanges();
    let numElemenets = fixture.debugElement.queryAll(By.css('.logout-link')).length;
    
    expect(numElemenets).not.toBe(0);
  });

  it('.user-profile-link should NOT be present in the components DOM when the user is NOT logged in', () => {  
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(false);
    fixture.autoDetectChanges();
    let numElemenets = fixture.debugElement.queryAll(By.css('.user-profile-link')).length;
    
    expect(numElemenets).toBe(0);
  });

  it('.user-profile-link should be present in the components DOM when the user is logged in', () => { 
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(true);
    fixture.autoDetectChanges();
    let numElements = fixture.debugElement.queryAll(By.css('.user-profile-link')).length;
    
    expect(numElements).not.toBe(0);
  });

  it('.login-link should be present in the components DOM when the user is NOT logged in', () => {
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(false);
    fixture.autoDetectChanges();
    let numElements = fixture.debugElement.queryAll(By.css('.login-link')).length;
    
    expect(numElements).not.toBe(0);
  });


  it('.login-link should NOT be present in the components DOM when the user is logged in', () => { 
    spyOn(TestBed.get(AuthService), 'isLoggedIn').and.returnValue(true);
    fixture.autoDetectChanges();
    let numElemenets = fixture.debugElement.queryAll(By.css('.login-link')).length;
    
    expect(numElemenets).toBe(0);
  });
  
});
