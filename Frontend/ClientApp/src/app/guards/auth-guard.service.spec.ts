import { TestBed, inject } from '@angular/core/testing';

import { AuthGuardService } from './auth-guard.service';
import { AuthService } from '../services/auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';


describe('AuthGuardService', () => {


  let routeSnapshot:any = jasmine.createSpyObj<ActivatedRouteSnapshot>("ActivatedRouteSnapshot", ['toString']);
  let mockSnapshot:any = jasmine.createSpyObj<RouterStateSnapshot>("RouterStateSnapshot", ['toString']);

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthGuardService, 
        { provide: AuthService, useValue: new AuthServiceMock(null) }]
    });


  });

  it('should be created', inject([AuthGuardService], (service: AuthGuardService) => {
    expect(service).toBeTruthy();
  }));

  it('canActivate return true when AuthService.isLoggedIn() returns true', inject([AuthGuardService], (service: AuthGuardService) => {
    let authService:AuthService = TestBed.get(AuthService);
    const spy = spyOn(authService, 'isLoggedIn').and.returnValue(true);
    
    expect(service.canActivate(routeSnapshot, mockSnapshot)).toBeTruthy();
  }));


  it('canActivate return true when AuthService.isLoggedIn() returns true', inject([AuthGuardService], (service: AuthGuardService) => {
    let authService:AuthService = TestBed.get(AuthService);
    const spy = spyOn(authService, 'isLoggedIn').and.returnValue(false);
    
    expect(service.canActivate(routeSnapshot, mockSnapshot)).toBeFalsy();
  }));


  it('canActivate should call AuthService.startAuthentication() when AuthService.isLoggedIn() returns false', inject([AuthGuardService], (service: AuthGuardService) => {
    let authService:AuthService = TestBed.get(AuthService);
    const spy = spyOn(authService, 'isLoggedIn').and.returnValue(false);
    const startAuthenticationSpy = spyOn(authService, 'startAuthentication')
    
    service.canActivate(routeSnapshot, mockSnapshot);

    expect(startAuthenticationSpy).toHaveBeenCalled();
  }));

  it('canActivate should NOT call AuthService.startAuthentication() when AuthService.isLoggedIn() returns true', inject([AuthGuardService], (service: AuthGuardService) => {
    let authService:AuthService = TestBed.get(AuthService);
    const spy = spyOn(authService, 'isLoggedIn').and.returnValue(true);
    const startAuthenticationSpy = spyOn(authService, 'startAuthentication')
    
    service.canActivate(routeSnapshot, mockSnapshot);

    expect(startAuthenticationSpy).not.toHaveBeenCalled();
  }));
});
