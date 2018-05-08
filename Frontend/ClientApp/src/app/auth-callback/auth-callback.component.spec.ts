import { TestBed, getTestBed, inject, ComponentFixture, tick, fakeAsync } from '@angular/core/testing';
import { AuthCallbackComponent } from './auth-callback.component';
import { AuthService } from '../services/auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';


describe('AuthCallbackComponent', () => {
  
  let fixture: ComponentFixture<AuthCallbackComponent>;
  let component: AuthCallbackComponent;
  let completeAuthenticationFnSpy: jasmine.Spy;
  

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      providers:[
        { provide: AuthService, useValue: new AuthServiceMock(null) }
      ],
      declarations: [ AuthCallbackComponent ]
    });
    let authService = TestBed.get(AuthService);
    completeAuthenticationFnSpy = spyOn(authService, 'completeAuthentication').and.returnValue(Promise.resolve<void>(null));
    fixture = TestBed.createComponent(AuthCallbackComponent);
    component = fixture.componentInstance;

  });



  
  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
  

  it('should call AuthService.completeAuthentication from components OnInit', () => {
    fixture.detectChanges();
    expect(completeAuthenticationFnSpy).toHaveBeenCalled();
    
  });
  
  it('should navigate to the default "" route after AuthService.completeAuthentication resolves', fakeAsync(() => { 
    let router: Router = TestBed.get(Router);
    let spy = spyOn(router, 'navigate');
    fixture.detectChanges();
    tick(); 
    expect(spy).toHaveBeenCalledWith(['']); 
  }));
  
});
